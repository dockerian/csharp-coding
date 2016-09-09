using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Library.Sorting;

namespace UnitTest
{
    /// <summary>
    /// Summary description for StackTest
    /// </summary>
    [TestClass]
    public class FooTest
    {
        public FooTest()
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

        #region Functions

        private void LoadArray(out int[] array)
        {
            int size;
            Random rand = new Random(DateTime.Now.Millisecond);

            size = rand.Next(100);
            array = new int[size];

            for(int i = 0; i < size; i++)
            {
                int data = rand.Next();
                int division = rand.Next(1, 10);
                array[i] = data % division == 0 ? data : -data;
            }
        }

        private void LoadList(out List<int> aList)
        {
            int size;
            Random rand = new Random(DateTime.Now.Millisecond);

            size = rand.Next(100);
            aList = new List<int>();

            for(int i = 0; i < size; i++)
            {
                int data = rand.Next();
                int division = rand.Next(1, 10);
                int dataValue = data % division == 0 ? data : -data;
                aList.Add(dataValue);
            }
        }

        private bool IsArraySorted(int[] array, bool decending = false)
        {
            if (array == null) return false;
            if (array.Length < 2) return true;

            for(int i = 0; i < array.Length - 1; i++)
            {
                if (decending && array[i] > array[i + 1] || !decending && array[i] > array[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsListSorted(List<int> aList, bool decending = false)
        {
            if (aList == null) return false;
            if (aList.Count < 2) return true;

            for (int i = 0; i < aList.Count - 1; i++)
            {
                if (decending && aList[i] > aList[i + 1] || !decending && aList[i] > aList[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        [TestMethod]
        public void FoobarTest()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public void FooTests()
        {

        }

    }
}
