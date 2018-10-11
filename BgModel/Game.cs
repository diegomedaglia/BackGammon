using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgModel
{
    public class Game
    {
        private Board board;
        private readonly Dice[] dices = new Dice[2];
        private readonly List<int> remainingMoves = new List<int>(4);

        public Checker.CheckerColor CurrentPlayer { get; set; }

        bool GameStarted = false;

        public Game()
        {
            board = Board.Instance;
        }

        public bool NewGame()
        {
            if (GameStarted)
                board.ResetBoard();

            GameStarted = false;

            RollDices();

            GameStarted = Dice1Value != Dice2Value;

            if (GameStarted)
            {
                CurrentPlayer = Dice1Value > Dice2Value ? 
                    Checker.CheckerColor.White : 
                    Checker.CheckerColor.Black;
            }

            return GameStarted;
        }


        public int Dice1Value
        {
            get
            {
                return dices[0].Value;
            }
        }

        public int Dice2Value
        {
            get
            {
                return dices[1].Value;
            }
        }

        public void RollDices()
        {
            remainingMoves.Clear();

            foreach (var dice in dices)
            {
                dice.RollDice();
            }

            remainingMoves.Add(Dice1Value);
            remainingMoves.Add(Dice2Value);

            // Doubles
            if (Dice1Value == Dice2Value)
            {
                remainingMoves.Add(Dice1Value);
                remainingMoves.Add(Dice2Value);
            }

            remainingMoves.Sort();
        }

        public bool CanBearOffChecker(Checker checker)
        {
            bool ret = false;

            if (remainingMoves.Count != 0)
            {
                int point = 0;

                if (checker.Color == Checker.CheckerColor.White)
                {
                    point = Math.Abs(checker.Point - 24);
                    ret = board.WhiteCanBearOff;
                }
                else
                {
                    point = checker.Point;
                    ret = board.BlackCanBearOff;
                }

                if (ret)
                {
                    ret = remainingMoves.Contains(point) || remainingMoves[0] > point;
                }
            }

            return ret;
        }

        public bool CanCheckerReachPoint(Checker checker, int destPoint)
        {
            bool ret = false;

            if (checker.Color == Checker.CheckerColor.White)
            {
                foreach (int move in remainingMoves)
                {
                    // Natural
                    if (checker.Point + move == destPoint)
                    {
                        ret = true;
                        break;
                    }
                }

                // Composite move
                if (!ret)
                {

                    if (remainingMoves.Count == 2)
                    {
                        // not a double: the checker goes to one point using one of the remaining moves
                        // then to the final destination point 
                        ret = remainingMoves.Sum() + checker.Point == destPoint &&
                                (board.CanMoveChecker(checker, checker.Point + remainingMoves[0])
                                    || board.CanMoveChecker(checker, checker.Point + remainingMoves[1]));
                    }
                    else
                    {
                        if (destPoint - checker.Point % remainingMoves[0] == 0)
                        {
                            // it's possible to reach, let's figure out if there's a path
                            int pipsLeft = destPoint - checker.Point;

                            ret = true;
                            int movesLeft = remainingMoves.Count;

                            while (pipsLeft > 0)
                            {
                                if (board.CanMoveChecker(checker, checker.Point + remainingMoves[0]))
                                {
                                    movesLeft--;
                                    pipsLeft -= remainingMoves[0];
                                }
                                else
                                {
                                    ret = false;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                foreach (int move in remainingMoves)
                {
                    // Natural
                    if (checker.Point - move == destPoint)
                    {
                        ret = true;
                        break;
                    }
                }

                // Composite move
                if (!ret)
                {
                    if (remainingMoves.Count == 2)
                    {
                        // not a double: the checker goes to one point using one of the remaining moves
                        // then to the final destination point 
                        ret = checker.Point - remainingMoves.Sum() == destPoint &&
                                (board.CanMoveChecker(checker, checker.Point - remainingMoves[0])
                                    || board.CanMoveChecker(checker, checker.Point - remainingMoves[1]));
                    }
                    else
                    {
                        // doubles: let's see if the checker can land at every intermediate point
                        // before reaching the destination point
                        int pipsLeft = checker.Point - destPoint;

                        if (pipsLeft % remainingMoves[0] == 0)
                        {
                            // it's possible to reach, let's figure out if there's a path

                            ret = true;
                            int movesLeft = remainingMoves.Count;

                            while (pipsLeft > 0)
                            {
                                if (board.CanMoveChecker(checker, checker.Point - remainingMoves[0]))
                                {
                                    movesLeft--;
                                    pipsLeft -= remainingMoves[0];
                                }
                                else
                                {
                                    ret = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public bool CanMoveChecker(Checker checker, int destPoint)
        {
            bool ret = false;

            if (destPoint >= 0 && destPoint < 24 &&
                checker.Color == CurrentPlayer)
            {
                ret = board.CanMoveChecker(checker, destPoint) &&
                    CanCheckerReachPoint(checker, destPoint);
            }

            return ret;
        }

        public bool MoveChecker(Checker checker, int destPoint)
        {
            bool ret = false;
            if (CanMoveChecker(checker, destPoint))
            {
                int originalCheckerPoint = checker.Point;

                ret = board.MoveChecker(checker, destPoint);

                if(ret)
                {
                    int pipsLeft;

                    if (checker.Color == Checker.CheckerColor.White)
                        pipsLeft = destPoint - originalCheckerPoint;
                    else
                        pipsLeft = originalCheckerPoint - destPoint;

                    // figure out how many reamining moves we should remove
                    if (pipsLeft > 6)
                    {
                        // more than one move for sure: pips left > 6

                        // optimization: just two moves left will require all moves
                        if (remainingMoves.Count == 2)
                        {
                            remainingMoves.Clear();
                        }
                        else
                        {
                            // remove as many moves as it takes to reach the point
                            while (pipsLeft > 0)
                            {
                                pipsLeft -= remainingMoves[0];
                                remainingMoves.RemoveAt(0);
                            }
                        }
                    }
                    else // 6 or less pips left
                    {
                        
                        if (remainingMoves.Count == 1)
                        {
                            // just 1 move left, no need to sum it first
                            remainingMoves.Clear();
                        }
                        else
                        {
                            if (remainingMoves.Sum() == pipsLeft)
                            {
                                // it will take all remaining moves to reach the point
                                remainingMoves.Clear();
                            }
                            else
                            {
                                // we have more than enough moves to reach the pip, remove the first occurence
                                // of the natural to reach there.
                                remainingMoves.Remove(pipsLeft);
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
