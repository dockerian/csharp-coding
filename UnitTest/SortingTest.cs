using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Extensions;
using Common.Library.Sorting;
using Common.Library;

namespace UnitTest
{
    /// <summary>
    /// Summary description for StackTest
    /// </summary>
    [TestClass]
    public class SortingTest
    {
        public SortingTest()
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

        private bool IsArraySorted<T>(T[] array, bool decending = false) where T : IComparable
        {
            /*//
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
            //*/
            return array.IsSorted(decending);
        }

        private bool IsListSorted<T>(List<T> aList, bool decending = false) where T : IComparable
        {
            /*//
            if (aList == null) return false;
            if (aList.Count < 2) return true;

            for (int i = 0; i < aList.Count - 1; i++)
            {
                if (decending && aList[i].CompareTo(aList[i + 1]) > 0 || !decending && aList[i].CompareTo(aList[i + 1]) < 0)
                {
                    return false;
                }
            }
            return true;
            //*/
            return aList.IsSorted(decending);
        }

        #endregion

        [TestMethod]
        public void FooTest()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public void SortingTests()
        {
            int[] array;
            List<int> aList;

            LoadArray(out array);
            BubbleSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadList(out aList);
            BubbleSort.Sort(aList);
            Assert.IsTrue(IsListSorted(aList));

            LoadArray(out array);
            HeapSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadArray(out array);
            InsertionSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadArray(out array);
            MergeSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadList(out aList);
            MergeSort.Sort(aList);
            Assert.IsTrue(IsListSorted(aList));

            LoadArray(out array);
            QuickSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadArray(out array);
            SelectionSort.Sort(array);
            Assert.IsTrue(IsArraySorted(array));

            LoadList(out aList);
            SelectionSort.Sort(aList);
            Assert.IsTrue(IsListSorted(aList));

            LoadList(out aList);
            StoogeSort.Sort(aList);
            Assert.IsTrue(IsListSorted(aList));

        }

    }
}
