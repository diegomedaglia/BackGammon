using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgModel
{
    public class Checker
    {
        //private static int id = 0;

        //private int myId;

        public enum CheckerColor
        {
            White,
            Black
        }

        public CheckerColor Color { get; set; }

        private int point;

        public int Point
        {
            get { return point; }
            set
            {
                if (point >= 0 && point < 24)
                    point = value;
            }
        }
        
        public Checker() : this(CheckerColor.White)
        {
            
        }

        public Checker(CheckerColor color)
        {
            Color = color;
            //myId = id++;
        }

        //public override bool Equals(object obj)
        //{
        //    return myId == ((Checker) obj).myId;
        //}
    }
}
