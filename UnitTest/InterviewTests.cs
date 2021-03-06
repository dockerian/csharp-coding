﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Extensions;
using Common.Library;
using Coding.App;
using Coding.App.Interviews;


namespace UnitTest
{
    ///<summary>
    ///This is a test class for QuestionsTest and is intended
    ///to contain all QuestionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InterviewTests
    {
        #region :: Private Fields

        private TestContext testContextInstance;

        #endregion

        #region :: Properties
        ///<summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region :: Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }

        #endregion

        #region :: Tests

        ///<summary>
        ///A test method
        ///</summary>
        [TestMethod()]
        public void A_Foo_Test()
        {
        }

        ///<summary>
        ///A test method
        ///</summary>
        [TestMethod()]
        public void IntMatrixAjacentTest()
        {
            int[,] array = new int[,]
            {
                { 1, 1, 0, 1, 0, 1 },
                { 1, 1, 1, 1, 0, 1 },
                { 0, 0, 0, 0, 1, 1 },
                { 1, 0, 1, 0, 1, 0 },
            };

            Coding.App.Interviews.IntMatrix matrix = new Coding.App.Interviews.IntMatrix(array);

            List<IntMatrixCoordinate> expected = new List<IntMatrixCoordinate>();
            List<IntMatrixCoordinate> result = new List<IntMatrixCoordinate>();

            expected.Add(new IntMatrixCoordinate(0,0));
            expected.Add(new IntMatrixCoordinate(1,0));
            expected.Add(new IntMatrixCoordinate(0,1));
            expected.Add(new IntMatrixCoordinate(1,1));
            expected.Add(new IntMatrixCoordinate(1,2));
            expected.Add(new IntMatrixCoordinate(1,3));
            expected.Add(new IntMatrixCoordinate(0,3));
            expected.Add(new IntMatrixCoordinate(0,5));
            expected.Add(new IntMatrixCoordinate(1,5));
            expected.Add(new IntMatrixCoordinate(2,4));
            expected.Add(new IntMatrixCoordinate(2,5));
            expected.Add(new IntMatrixCoordinate(3,4));
            expected.Add(new IntMatrixCoordinate(3,0));
            expected.Add(new IntMatrixCoordinate(3,2));

            result = matrix.FindAdjacent1s().ToList();

            Assert.AreEqual(result.Count(), expected.Count());

            foreach (var coordinate in result)
            {
                var match = expected.FirstOrDefault(a => a.X == coordinate.X && a.Y == coordinate.Y);

                Assert.IsNotNull(match);
            }

        }

        #endregion
    }
}
