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

        private bool CanCheckerReachPoint(Checker checker, int destPoint)
        {
            bool ret = false;

            List<int> possibleMoves = new List<int>(remainingMoves);

            for (var i = 0; i < possibleMoves.Count; i++)
            {
                // TODO
            }

            if (checker.Color == Checker.CheckerColor.White)
            {
                
            }

            return ret;
        }

        public bool CanMoveChecker(Checker checker, int destPoint)
        {
            bool ret = false;

            if (destPoint >= 0 && destPoint < 24 &&
                checker.Color == CurrentPlayer)
            {
                ret = board.CanMoveChecker(checker, destPoint);
            }

            return ret;
        }

        public bool MoveChecker(Checker checker, int destPoint)
        {
            bool ret = false;
            if (CanMoveChecker(checker, destPoint))
                ret = board.MoveChecker(checker, destPoint);

            return ret;
        }
    }
}
