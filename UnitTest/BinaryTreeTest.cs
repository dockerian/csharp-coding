using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Library;
using Coding.App;


namespace UnitTest
{
    ///<summary>
    ///This is a test class for BinaryTreeTest and is intended
    ///to contain all BinaryTreeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BinaryTreeTest
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
        ///A test method for BinaryTree
        ///</summary>
        [TestMethod()]
        public void BinaryTreeConstructorTest()
        {
            BinaryTreeNode<double> node0 = new BinaryTreeNode<double>(20.0);
            BinaryTreeNode<double> node1 = new BinaryTreeNode<double>(20.1);
            BinaryTreeNode<double> node2 = new BinaryTreeNode<double>(20.2);

            Assert.AreEqual(node1.CompareTo(node2), -1);
            Assert.AreEqual(node1.CompareTo(node2.Data), -1);
            Assert.AreEqual(node2.CompareTo(node2.Data), 0);


        }

        #endregion
    }
}
