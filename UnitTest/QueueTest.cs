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
    public class QueueTest
    {
        public QueueTest()
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
        public void QueueTests()
        {
            bool hasException = false;
            object peekObject = new object();
            object dequObject = new object();
            QueueGeneric<int?> queue1 = new QueueGeneric<int?>();
            QueueComparable<int> queue2 = new QueueComparable<int>();
            QueueComparable<string> queue3 = new QueueComparable<string>();
            DoubleStackQueue<string> queue4 = new DoubleStackQueue<string>();

            #region QueueTest :: empty Queue<non-nullable>

            try
            {
                peekObject = queue2.Peek(); // peek an empty queue
            }
            catch(QueueOperationException e)
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
                dequObject = queue2.Dequeue(); // operate on empty queue
            }
            catch (QueueOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message);
            }
            finally
            {
                Assert.IsNotNull(dequObject);
                Assert.IsTrue(hasException);
            }

            #endregion

            #region QueueTest :: empty Queue<nullable>

            hasException = false;
            try
            {
                peekObject = queue1.Peek();
            }
            catch (QueueOperationException e)
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
                var test1 = "Queue3.test1";
                queue3.Enqueue(test1);
                peekObject = queue3.Peek();
                dequObject = queue3.Dequeue();
                Assert.IsTrue(dequObject.Equals(peekObject));
                Assert.IsTrue(dequObject.Equals(test1));
                Assert.IsNotNull(dequObject);

                dequObject = queue3.Dequeue();
            }
            catch (QueueOperationException e)
            {
                hasException = true;
                Assert.IsNotNull(e.Message); //should never come here
            }
            finally
            {
                Assert.IsFalse(hasException);
                Assert.IsNull(dequObject);
            }

            hasException = false;
            try
            {
                peekObject = queue3.Peek();
            }
            catch (QueueOperationException e)
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

            #region QueueTest :: Get the minimum in the Queue

            var test3_1 = "";
            var test3_2 = "ABCD";
            var test3_3 = "abcd";
            var test3_4 = "xyz...12345";
            var test3_5 = "5555";

            queue3.Enqueue(test3_1);
            queue4.Enqueue(test3_1);
            Assert.AreEqual(queue4.First, queue3.First);
            Assert.AreEqual(queue4.Last, queue3.Last);
            Assert.IsTrue(test3_1.Equals(queue3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(queue3.Minimum));

            queue3.Enqueue(test3_2);
            queue4.Enqueue(test3_2);
            Assert.AreEqual(queue4.First, queue3.First);
            Assert.AreEqual(queue4.Last, queue3.Last);
            Assert.IsTrue(test3_1.Equals(queue3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(queue3.Minimum));

            queue3.Enqueue(test3_3);
            queue4.Enqueue(test3_3);
            Assert.AreEqual(queue4.First, queue3.First);
            Assert.AreEqual(queue4.Last, queue3.Last);
            Assert.IsTrue(test3_1.Equals(queue3.GetMinimum()));
            Assert.IsTrue(test3_1.Equals(queue3.Minimum));

            queue3.Dequeue();
            queue4.Dequeue();
            Assert.AreEqual(queue4.Peek(), queue3.Peek());

            queue3.Enqueue(test3_4);
            queue4.Enqueue(test3_4);
            Assert.AreEqual(queue4.First, queue3.First);
            Assert.AreEqual(queue4.Last, queue3.Last);
            Assert.IsTrue(test3_3.Equals(queue3.GetMinimum()));
            Assert.IsTrue(test3_3.Equals(queue3.Minimum));

            queue3.Enqueue(test3_5);
            queue4.Enqueue(test3_5);
            Assert.AreEqual(queue4.First, queue3.First);
            Assert.AreEqual(queue4.Last, queue3.Last);
            Assert.IsTrue(test3_5.Equals(queue3.GetMinimum()));
            Assert.IsTrue(test3_5.Equals(queue3.Minimum));

            string q3string = queue3.ToString();
            string q4string = queue4.ToString();
            Assert.AreEqual(q3string, q4string);

            #endregion

            #region QueueTest ::

            #endregion

        }

    }
}
