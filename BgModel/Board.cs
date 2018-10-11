using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgModel
{
    public class Board
    {
        private static Board instance = null;

        private List<Checker> whiteCheckers = new List<Checker>(15);
        private List<Checker> blackCheckers = new List<Checker>(15);

        /// <summary>
        /// Represents each point of the board. point 0 is black's ace point, 
        /// point 23 is white's ace point.
        /// </summary>
        private readonly List<Checker>[] points = new List<Checker>[24];
        private List<Checker> whiteBar = new List<Checker>();
        private List<Checker> blackBar = new List<Checker>();
        private readonly List<Checker> whiteBearOff = new List<Checker>();
        private readonly List<Checker> blackBearOff = new List<Checker>();

        public bool WhiteCanBearOff { get; private set; } = false;
        public bool BlackCanBearOff { get; private set; } = false;
        public static Board Instance
        {
            get
            {
                if (instance == null)
                    instance = new Board();

                return instance;
            }
        }

        public bool IsPointMade(int point)
        {
            bool ret = false;

            if (point >= 0 && point <= 23)
                ret = points[point].Count > 1;

            return ret;
        }

        public ref List<Checker> this[int key]
        {
            get { return ref points[key]; }
        }

        public bool CanMoveChecker(Checker checker, int destPoint)
        {
            return CanMoveChecker(checker.Point, destPoint);
        }

        public bool CanMoveChecker(int sourcePoint, int destPoint)
        {
            bool ret = false;

            if (points[sourcePoint].Count > 0)
            {
                if (points[destPoint].Count == 0)
                {
                    ret = true;
                }
                else
                {
                    Checker.CheckerColor sourceColor = points[sourcePoint][0].Color;

                    if (sourceColor == points[destPoint][0].Color)
                    {
                        ret = true;
                    }
                    else
                    {
                        if (points[destPoint].Count == 1)
                        {
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }


        private Board()
        {
            for (int i = 0; i < 15; i++)
            {
                whiteCheckers.Add(new Checker() { Color = Checker.CheckerColor.White });
                blackCheckers.Add(new Checker() { Color = Checker.CheckerColor.Black });
            }

            for (int i = 0; i < 24; i++)
            {
                points[i] = new List<Checker>();
            }

            ResetBoard();
        }

        private void RefreshBearOff(Checker.CheckerColor color)
        {
            int sum = 0;

            if (color == Checker.CheckerColor.White)
            {
                for (int i = 18; i < 24; i++)
                    sum += points[i].Count;

                WhiteCanBearOff = sum == 15;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    sum += points[i].Count;

                BlackCanBearOff = sum == 15;
            }
        }

        public bool MoveChecker(Checker checker, int destPoint)
        {
            bool ret = false;

            if (CanMoveChecker(checker, destPoint))
            {
                points[destPoint].Add(checker);
                bool r = points[checker.Point].Remove(checker);

                checker.Point = destPoint;

                RefreshBearOff(checker.Color);

                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Resets the board ot the starting position:
        /// - the bar is empty
        /// - checkers are in the starting position:
        /// 
        /// |-------------------------------------|
        /// |11 10 09 08 07 06 | 05 04 03 02 01 00|
        /// |w           b     | b              w |
        /// |w           b     | b              w |
        /// |w           b     | b                |
        /// |w                 | b                |
        /// |w                 | b                |
        /// |------------------|------------------|
        /// |b                 | w                |
        /// |b                 | w                |
        /// |b           w     | w                |
        /// |b           w     | w              b |
        /// |b           w     | w              b |
        /// |12 13 14 15 16 17 | 18 19 20 21 21 23|
        /// |-------------------------------------|
        /// </summary>
        public void ResetBoard()
        {
            int blackIndex = 0;
            int whiteIndex = 0;

            foreach (var item in points)
            {
                item.Clear();
            }

            whiteBar.Clear();
            blackBar.Clear();

            whiteBearOff.Clear();
            blackBearOff.Clear();

            WhiteCanBearOff = false;
            BlackCanBearOff = false;

            whiteCheckers[whiteIndex].Point = 0;
            points[0].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 0;
            points[0].Add(whiteCheckers[whiteIndex++]);

            blackCheckers[blackIndex].Point = 5;
            points[5].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 5;
            points[5].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 5;
            points[5].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 5;
            points[5].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 5;
            points[5].Add(blackCheckers[blackIndex++]);

            blackCheckers[blackIndex].Point = 7;
            points[7].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 7;
            points[7].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 7;
            points[7].Add(blackCheckers[blackIndex++]);

            whiteCheckers[whiteIndex].Point = 11;
            points[11].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 11;
            points[11].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 11;
            points[11].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 11;
            points[11].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 11;
            points[11].Add(whiteCheckers[whiteIndex++]);

            blackCheckers[blackIndex].Point = 12;
            points[12].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 12;
            points[12].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 12;
            points[12].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 12;
            points[12].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 12;
            points[12].Add(blackCheckers[blackIndex++]);

            whiteCheckers[whiteIndex].Point = 16;
            points[16].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 16;
            points[16].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 16;
            points[16].Add(whiteCheckers[whiteIndex++]);

            whiteCheckers[whiteIndex].Point = 18;
            points[18].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 18;
            points[18].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 18;
            points[18].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 18;
            points[18].Add(whiteCheckers[whiteIndex++]);
            whiteCheckers[whiteIndex].Point = 18;
            points[18].Add(whiteCheckers[whiteIndex++]);

            blackCheckers[blackIndex].Point = 23;
            points[23].Add(blackCheckers[blackIndex++]);
            blackCheckers[blackIndex].Point = 23;
            points[23].Add(blackCheckers[blackIndex++]);

            // make sure we assigned all checkers above
            Debug.Assert(whiteIndex == 15);
            Debug.Assert(blackIndex == 15);
        }

        public string GetCurrentBoard()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("|-------------------------------------|");
            sb.AppendLine("|11 10 09 08 07 06 | 05 04 03 02 01 00|");

            sb.Append("|");

            for (int i = 11; i >= 0; i--)
            {
                if (points[i].Count > 0)
                {
                    sb.Append(points[i].Count);
                    sb.Append(points[i][0].Color == Checker.CheckerColor.White ? "w" : "b");
                }
                else
                {
                    sb.Append("  ");
                }

                if (i == 6)
                    sb.Append("  ");

                if (i > 0)
                    sb.Append(" ");
            }
            sb.AppendLine("|");
            sb.AppendLine("|------------------|------------------|");

            sb.Append("|");

            for (int i = 12; i < 24; i++)
            {
                if (points[i].Count > 0)
                {
                    sb.Append(points[i].Count);
                    sb.Append(points[i][0].Color == Checker.CheckerColor.White ? "w" : "b");
                }
                else
                {
                    sb.Append("  ");
                }

                if (i == 17)
                    sb.Append("  ");

                if (i < 23)
                    sb.Append(" ");
            }
            sb.AppendLine("|");
            sb.AppendLine("|12 13 14 15 16 17 | 18 19 20 21 21 23|");
            sb.AppendLine("|-------------------------------------|");

            sb.AppendLine();

            sb.AppendLine($"white bar: {whiteBar.Count}");
            sb.AppendLine($"black bar: {blackBar.Count}");

            sb.AppendLine($"white beared off: {whiteBearOff.Count}");
            sb.AppendLine($"black beared off: {blackBearOff.Count}");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
