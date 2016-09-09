using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Library;

namespace UnitTest
{
    /// <summary>
    /// Summary description for StackTest
    /// </summary>
    [TestClass]
    public class HanoiTowerTest
    {
        public HanoiTowerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void FooTest()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public void HanoiTowerSolutionTest()
        {
            string[] dataTest1 = new string[]
                {
                    "2 3",
                    "1 1",
                    "2 2",
                };
            string[] expected1 = new string[]
                {
                    "3",
                    "1 3",
                    "1 2",
                    "3 2",
                };
            string[] dataTest2 = new string[]
                {
                    "6 4",
                    "4 2 4 3 1 1",
                    "1 1 1 1 1 1",
                };
            string[] expected2 = new string[]
                {
                    "5",
                    "3 1",
                    "4 3",
                    "4 1",
                    "2 1",
                    "3 1",
                };

            string[] result1 = Solution.Start(dataTest1);

            for(int i = 0; i < expected1.Length; i++)
            {
                Assert.AreEqual(result1[i], expected1[i]);
            }

            string[] result2 = Solution.Start(dataTest2);

            for(int i = 0; i < expected2.Length; i++)
            {
                Assert.AreEqual(result2[i], expected2[i]);
            }

        }

    }
}
