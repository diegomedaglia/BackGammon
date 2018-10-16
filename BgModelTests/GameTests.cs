using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BgModel;

namespace BgModelTests
{
    [TestClass]
    public class GameTests
    {
        public TestContext TestContext { get; set; }
        Game g;

        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void NewGameTest()
        {
            bool started = false;

            Game g = new Game();

            while (!started)
                started = g.NewGame();

            TestContext.WriteLine(Enum.GetName(g.CurrentPlayer.GetType(), g.CurrentPlayer));
        }

        [TestMethod]
        public void NewGameLoopTest()
        {
            Game g = new Game();
            int same = 0;
            int white = 0;
            int black = 0;
            bool started = false;
            int totalRuns = 1000;

            for (var i = 0; i < totalRuns; i++)
            {
                started = g.NewGame();

                if (!started)
                    same++;
                else if (g.CurrentPlayer == Checker.CheckerColor.White)
                    white++;
                else
                    black++;
            }

            TestContext.WriteLine("Results:");
            TestContext.WriteLine($"\tSame: {same} ({((float)same * 100/totalRuns).ToString("0.00")}%)");
            TestContext.WriteLine($"\tSame: {white} ({((float)white * 100 / totalRuns).ToString("0.00")}%)");
            TestContext.WriteLine($"\tSame: {black} ({((float)black * 100 / totalRuns).ToString("0.00")}%)");
            
        }

        /// <summary>
        /// Starts a new game until the dice are not the same.
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private Checker.CheckerColor InitializeMoveTest(Game g)
        {
            bool started;

            do
            {
                started = g.NewGame();
            } while (!started);

            return g.CurrentPlayer;
        }

        /// <summary>
        /// Starts a new game until the starting player is the specified player
        /// </summary>
        /// <param name="g"></param>
        /// <param name="startingPlayer"></param>
        private void NewGameFixedStartingPlayer(Game g, Checker.CheckerColor startingPlayer)
        {
            do
            {
                InitializeMoveTest(g);

            } while (g.CurrentPlayer != startingPlayer);
        }

        [TestMethod]
        public void MoveWrongPlayerTest()
        {
            Game g = new Game();

            Checker c;

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.White);

            c = g.Board.GetPoint(7)[0];

            Assert.IsFalse(g.CanMoveChecker(c, c.Point - g.Dice1Value));
            Assert.IsFalse(g.MoveChecker(c, c.Point - g.Dice1Value));

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.Black);

            c = g.Board.GetPoint(16)[0];

            Assert.IsFalse(g.CanMoveChecker(c, c.Point + g.Dice1Value));
            Assert.IsFalse(g.MoveChecker(c, c.Point + g.Dice1Value));
        }

        [TestMethod]
        public void MoveRightPlayerTest()
        {
            Game g = new Game();

            Checker c;

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.White);

            c = g.Board.GetPoint(16)[0];

            Assert.IsTrue(g.CanMoveChecker(c, c.Point + g.Dice1Value));
            Assert.IsTrue(g.MoveChecker(c, c.Point + g.Dice1Value));

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.Black);

            c = g.Board.GetPoint(7)[0];

            Assert.IsTrue(g.CanMoveChecker(c, c.Point - g.Dice1Value));
            Assert.IsTrue(g.MoveChecker(c, c.Point - g.Dice1Value));
        }

        [TestMethod]
        public void MoveWrongDirection()
        {
            Game g = new Game();

            Checker c;

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.White);

            c = g.Board.GetPoint(18)[0];

            int val = g.Dice1Value == 6 ? g.Dice2Value : g.Dice1Value;

            Assert.IsFalse(g.CanMoveChecker(c, c.Point - val));
            Assert.IsFalse(g.MoveChecker(c, c.Point - val));

            NewGameFixedStartingPlayer(g, Checker.CheckerColor.Black);

            c = g.Board.GetPoint(5)[0];

            Assert.IsFalse(g.CanMoveChecker(c, c.Point + val));
            Assert.IsFalse(g.MoveChecker(c, c.Point + val));
        }
    }
}
