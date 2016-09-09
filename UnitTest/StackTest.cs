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
    public class StackTest
    {
        public StackTest()
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
        public void StackTests()
        {
            bool hasException = false;
            object peekObject = new object();
            object poppedItem = new object();
            StackGeneric<int?> stack1 = new StackGeneric<int?>();
            StackComparable<int> stack2 = new StackComparable<int>();
            StackComparable<string> stack3 = new StackComparable<string>();

            #region StackTest :: empty stack<non-nullable>

            try
            {
                peekObject = stack2.Peek();
            }
            catch(StackOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message);
            }
            finally
            {
                Assert.IsNotNull(peekObject);
                Assert.IsTrue(hasException);
            }

            hasException = false;
            try
            {
                poppedItem = stack2.Pop();
            }
            catch (StackOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message);
            }
            finally
            {
                Assert.IsNotNull(poppedItem);
                Assert.IsTrue(hasException);
            }

            #endregion

            #region StackTest :: empty stack<nullable>

            hasException = false;
            try
            {
                peekObject = stack1.Peek();
            }
            catch (StackOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message); //should never come here
            }
            finally
            {
                Assert.IsFalse(hasException);
                Assert.IsNull(peekObject);
            }

            hasException = false;
            try
            {
                var test1 = "stack3.test1";
                stack3.Push(test1);
                peekObject = stack3.Peek();
                poppedItem = stack3.Pop();
                Assert.IsTrue(poppedItem.Equals(peekObject));
                Assert.IsTrue(poppedItem.Equals(test1));
                Assert.IsNotNull(poppedItem);

                poppedItem = stack3.Pop();
            }
            catch (StackOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message); //should never come here
            }
            finally
            {
                Assert.IsFalse(hasException);
                Assert.IsNull(poppedItem);
            }

            hasException = false;
            try
            {
                peekObject = stack3.Peek();
            }
            catch (StackOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message); //should never come here
            }
            finally
            {
                Assert.IsFalse(hasException);
                Assert.IsNull(peekObject);
            }

            #endregion

            #region StackTest :: Get the minimum in the stack

            var test3_1 = "abcd";
            var test3_2 = "ABCD";
            var test3_3 = "";
            var test3_4 = "xyz...12345";
            var test3_5 = "5555";

            stack3.Push(test3_1);
            Assert.IsTrue(test3_1.Equals(stack3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(stack3.Minimum));

            stack3.Push(test3_2);
            Assert.IsTrue(test3_1.Equals(stack3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(stack3.Minimum));

            stack3.Push(test3_3);
            Assert.IsTrue(test3_3.Equals(stack3.GetMinimum()));
            Assert.IsTrue(test3_3.Equals(stack3.Minimum));

            stack3.Pop();

            stack3.Push(test3_4);
            Assert.IsTrue(test3_1.Equals(stack3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(stack3.Minimum));

            stack3.Push(test3_5);
            Assert.IsTrue(test3_5.Equals(stack3.GetMinimum()));
            Assert.IsTrue(test3_5.Equals(stack3.Minimum));

            #endregion

            #region StackTest ::

            #endregion

        }

    }
}
