
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BgModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BgModelTests
{
    [TestClass]
    public class BoardTest
    {
        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        Board b;

        [TestInitialize]
        public void Initialize()
        {
            b = Board.Instance;
            
        }


        [TestMethod]
        public void Board_GetBoardRepresentation()
        {
            TestContext.WriteLine(b.GetCurrentBoard());
        }

        [TestMethod]
        public void Board_CanMoveChecker()
        {
            List<Checker> point6 = b[5];

            Assert.IsTrue(b.CanMoveChecker(point6[0], 4));

            Assert.IsFalse(b.CanMoveChecker(point6[0], 0));
        }

        [TestMethod]
        public void Board_ResetBoard()
        {
            List<Checker> point6 = b[5];
            int point6count = point6.Count;

            b.MoveChecker(point6[0], 4);

            b.ResetBoard();

            Assert.IsTrue(point6.Count == point6count);
        }

        [TestMethod]
        public void Board_MoveChecker()
        {
            List<Checker> point6 = b[5];
            int point6count = point6.Count;

            b.MoveChecker(point6[0], 4);

            TestContext.WriteLine("After moving 65");
            TestContext.WriteLine(b.GetCurrentBoard());

            Assert.IsTrue(point6count > point6.Count);
            Assert.IsTrue(b[4].Count == 1);
        }

        [TestMethod]
        public void Board_IsPointMade()
        {
            b.ResetBoard();
            Assert.IsTrue(b.IsPointMade(0));
            Assert.IsTrue(b.IsPointMade(5));
            Assert.IsTrue(b.IsPointMade(7));
            Assert.IsTrue(b.IsPointMade(11));
            Assert.IsTrue(b.IsPointMade(12));
            Assert.IsTrue(b.IsPointMade(16));
            Assert.IsTrue(b.IsPointMade(18));
            Assert.IsTrue(b.IsPointMade(23));
        }
    }
}
