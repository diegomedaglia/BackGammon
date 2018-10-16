﻿using System;

namespace BgModel
{
    public class Dice
    {
        static private Random rng = new Random();

        public int Value { get; private set; }

        public void RollDice()
        {
            Value = rng.Next(1, 6);
        }
    }
}
