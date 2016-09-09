using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Extensions;
using Common.Library;
using Coding.App;


namespace UnitTest
{
    ///<summary>
    ///This is a test class for QuestionsTest and is intended
    ///to contain all QuestionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QuestionsTest
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
        ///A test method for array questions
        ///</summary>
        [TestMethod()]
        public void ArrayTest()
        {
            int[] array = new int[] { 4, 5, 6, 8, 3, 1, 4, 5, 6, 5, 4, 3, 1 };
            List<int> test1 = Questions.FindLongestSequenceArray(new List<int>(array));
            List<int> test2 = Questions.FindLongestSequenceArray(new List<int>(array), true, true);
            List<int> test3 = Questions.FindLongestSequenceArray(new List<int>());
            List<int> test4 = Questions.FindLongestSequenceArray(null);

            Assert.AreEqual(test1.Count, 4);
            Assert.AreEqual(test1.ToArray().IsSorted(), true);

            Assert.AreEqual(test2.Count, 4);
            Assert.AreEqual(test2.ToArray().IsSorted(true), true);

            Assert.AreEqual(test3.Count, 0);
            Assert.AreEqual(test4.Count, 0);
            Assert.IsNotNull(test4);

            array = new int[] {9, 9, 6, 1, 3, 4, 4, 5, 1, 7};
            List<int> test5 = Questions.FindOutOfSequence(new List<int>(array), out test3, true);
            List<int> test6 = Questions.FindOutOfSequence(new List<int>(array), out test4);

            Assert.IsTrue(test3.ToArray().IsSorted(true));
            Assert.IsTrue(test4.ToArray().IsSorted());

        }

        ///<summary>
        ///A test method for Bitwise questions
        ///</summary>
        [TestMethod()]
        public void BitsTest()
        {
            var range = 100;

            for(int number = 0; number < range; number++)
            {
                string s = Convert.ToString(number, 2).PadLeft(8, '0'); // convert to binary string

                int[] array = s.Select(c => int.Parse(c.ToString())) // convert each char to int
                             .ToArray();
                int countExpected = 0;

                for(int i = 0; i < array.Length; i++)
                {
                    if (array[i] == 1) countExpected++;
                }

                int countTest = Questions.CountNumberOfBits(number);
                Assert.AreEqual(countTest, countExpected);
            }
        }

        ///<summary>
        ///A test method
        ///</summary>
        [TestMethod()]
        public void CheckDuplicateTest()
        {
            int range = 10;
            int[] array = new int[] { 0, 9, 2, 10, 99, 3, 1, 2, 9, 4, 2 };
            int[] result = Questions.CheckDuplicate(array, range);

            Assert.AreEqual(result.Length, range);

            Questions.CheckDuplicate(array, range, true);

            for(int i = 0; i < range; i++)
            {
                Assert.AreEqual(array[i], result[i]);
            }
        }

        ///<summary>
        ///A test for DateTime Common Extensions
        ///</summary>
        [TestMethod()]
        public void DateTimeTest()
        {
            var now = DateTime.Now;
            var dt0 = new DateTime(now.Year, now.Month, now.Day,  0, 0, 0, DateTimeKind.Local);
            var dt1 = new DateTime(now.Year, now.Month, now.Day,  1, 0, 0, DateTimeKind.Local);
            var dt2 = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0, DateTimeKind.Local);
            var dt3 = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0, DateTimeKind.Local);
            var dt4 = new DateTime(now.Year, now.Month, now.Day, 4, 0, 0, DateTimeKind.Local);
            var dt5 = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0, DateTimeKind.Local);
            var dt6 = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0, DateTimeKind.Local);
            var dt7 = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0, DateTimeKind.Local);
            var dt8 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Local);
            var dt9 = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0, DateTimeKind.Local);

            var angle0 = Questions.GetAngle(dt0); //   0.0
            var angle1 = Questions.GetAngle(dt1); //  30.0
            var angle2 = Questions.GetAngle(dt2); //   0.0
            var angle3 = Questions.GetAngle(dt3); //  90.0
            var angle4 = Questions.GetAngle(dt4); // 120.0
            var angle5 = Questions.GetAngle(dt5); // 150.0
            var angle6 = Questions.GetAngle(dt6); // 180.0
            var angle7 = Questions.GetAngle(dt7); // 210.0
            var angle8 = Questions.GetAngle(dt8); // 240.0
            var angle9 = Questions.GetAngle(dt9); // 270.0

            Assert.AreEqual(angle4, Questions.GetAngle(dt8, true, true)); // 24-dials
            Assert.AreEqual(angle9, Questions.GetAngle(dt9));

            #region test result data for 90 degree

            /*//NOTE: 90 degree (all direction) between hour and minute hands (http://www.delphiforfun.org/programs/ClockAnglesPuzzle.htm)
                12:16.4
                12:49.1
                01:21.8
                01:54.5
                02:27.3
                03:00.0
                03:32.7
                04:05.4
                04:38.2
                05:10.9
                05:43.6
                06:16.4
                06:49.1
                07:21.8
                07:54.6
                08:27.3
                08:32.7
                09:00.0
                10:05.5
                10:38.2
                11:10.9
                11:43.6
            //*/

            #endregion

            int counterFound;
            KeyValuePair<DateTime, double>[] result0 = Generic.GetTimeByAngle(angle0, out counterFound);
            Assert.AreEqual(result0.Length, 11);
            Assert.AreEqual(counterFound, 11);

            KeyValuePair<DateTime, double>[] result1 = Generic.GetTimeByAngle(angle1, out counterFound);
            Assert.AreEqual(result1.Length, 22);
            Assert.AreEqual(counterFound, 22);

            KeyValuePair<DateTime, double>[] result2 = Generic.GetTimeByAngle(angle2, out counterFound);
            Assert.AreEqual(result2.Length, 11);
            Assert.AreEqual(counterFound, 11);

            KeyValuePair<DateTime, double>[] result3 = Generic.GetTimeByAngle(angle3, out counterFound);
            Assert.AreEqual(result3.Length, 22);
            Assert.AreEqual(counterFound, 22);

            KeyValuePair<DateTime, double>[] result4 = Generic.GetTimeByAngle(angle4, out counterFound);
            Assert.AreEqual(result4.Length, 22);
            Assert.AreEqual(counterFound, 22);

            KeyValuePair<DateTime, double>[] result5 = Generic.GetTimeByAngle(angle5, out counterFound);
            Assert.AreEqual(result5.Length, 22);
            Assert.AreEqual(counterFound, 22);

            KeyValuePair<DateTime, double>[] result6 = Generic.GetTimeByAngle(angle6, out counterFound);
            Assert.AreEqual(result6.Length, 11);
            Assert.AreEqual(counterFound, 11);

            KeyValuePair<DateTime, double>[] result8 = Generic.GetTimeByAngle(angle8, out counterFound);
            Assert.AreEqual(result8.Length, 22);
            Assert.AreEqual(counterFound, 22);

            result3 = Generic.GetTimeByAngle(angle3 + 360, out counterFound, false, true);
            Assert.AreEqual(result3.Length, 44);

            result6 = Generic.GetTimeByAngle(angle6, out counterFound, false, true);
            Assert.AreEqual(result6.Length, 22);

            result5 = Generic.GetTimeByAngle(angle5, out counterFound, false, true);
            Assert.AreEqual(result5.Length, 44);

            result8 = Generic.GetTimeByAngle(angle8, out counterFound, false, true);
            Assert.AreEqual(result8.Length, 44);
        }

        ///<summary>
        ///A test method for GetDoorStatus
        ///</summary>
        [TestMethod()]
        public void DoorStatusTest()
        {
            for(int x = 0; x <= 10; x++)
            {
                int num = x * x;
                bool[] result = Questions.GetDoorsStatus(num);

                if (num == 0)
                {
                    Assert.IsNull(result);
                    continue;
                }

                int imod = num % 2;
                //NOTE: if num is odd, the first door is opened (true); otherwise, closed (false).
                Assert.AreEqual(result[0], imod == 1);

                for(int i = 1; i < num; i++)
                {
                    int isqrt = (int) Math.Sqrt(i);
                    int isquare = isqrt*isqrt;

                    //NOTE: if index is a square of integer, the door is closed (false); otherwise, opened (true)
                    Assert.AreEqual(result[i], (i != isquare));
                }
            }
        }

        ///<summary>
        ///A test method for GetMaxSubArray()
        ///</summary>
        [TestMethod()]
        public void GetMaxSubArrayTest()
        {
            int[] array0 = new int[] {};
            int[] array1 = new int[] { -5, 0, 9, -1, -3, 3, 20, 7, 0, 5};
            int[] array2 = new int[] { -5, 0, -9, -1, -3, -3, -20, -7, 0, -5 };
            int[] array3 = new int[] { 0, 9, 1, 3, 3, -20, 7, 0, 5 };
            int[] array4 = new int[] { -9, -1, -3, -3, -2, -7, -5 };
            int[] array5 = new int[] { -9, -1, -30, 30, -2, 0, 5, 24 };

            int expectV0 = 0;
            int expectV1 = 35;
            int expectV2 = 0;
            int expectV3 = 16;
            int expectV4 = -1;
            int expectV5 = 30;

            int testMax0, testMax1, testMax2, testMax3, testMax4, testMax5;

            Assert.IsNull(Questions.GetMaxSubArray(null, out testMax0));
            Assert.AreEqual(expectV0, testMax0);

            int[] atest0 = Questions.GetMaxSubArray(array0, out testMax0);
            int[] atest1 = Questions.GetMaxSubArray(array1, out testMax1);
            int[] atest2 = Questions.GetMaxSubArray(array2, out testMax2);
            int[] atest3 = Questions.GetMaxSubArray(array3, out testMax3);
            int[] atest4 = Questions.GetMaxSubArray(array4, out testMax4);
            int[] atest5 = Questions.GetMaxSubArray(array5, out testMax5);

            Assert.AreEqual(expectV0, testMax0);
            Assert.AreEqual(expectV1, testMax1);
            Assert.AreEqual(expectV2, testMax2);
            Assert.AreEqual(expectV3, testMax3);
            Assert.AreEqual(expectV4, testMax4);
            Assert.AreEqual(expectV5, testMax5);

        }

        ///<summary>
        ///A test method for using Linked List as number
        ///</summary>
        [TestMethod()]
        public void LinkedListTest()
        {
            uint[] array0 = new uint[] { 0, 1, 2, 3, 4, 5, 6 };    // 6,543,210
            uint[] array1 = new uint[] { 0, 2, 4, 6, 8, 0, 3, 1 }; //13,086,420
            uint[] array2 = new uint[] { 9, 8, 7 };                //       789
            uint[] array3 = new uint[] { };
            uint[] array4 = new uint[] { 100 };                    //       100
            uint[] array5 = new uint[] { 0, 1, 3, 3, 4, 5, 6 };    // 6,543,310
            uint[] array6 = new uint[] { 9, 8, 8 };                //       889
            uint[] array7 = new uint[] { 0, 0, 1 };                //       100
            uint[] array8 = new uint[] { 8, 7, 7, 1 };             //     1,778

            SingleLinkedList<uint> List0 = new SingleLinkedList<uint>(array0);
            SingleLinkedList<uint> List1 = new SingleLinkedList<uint>(array1);
            SingleLinkedList<uint> List2 = new SingleLinkedList<uint>(array2);
            SingleLinkedList<uint> List3 = new SingleLinkedList<uint>(array3);
            SingleLinkedList<uint> List4 = new SingleLinkedList<uint>(array4);
            SingleLinkedList<uint> List5 = new SingleLinkedList<uint>(array5);
            SingleLinkedList<uint> List6 = new SingleLinkedList<uint>(array6);

            SingleLinkedList<uint> test1 = Questions.AddListsAsNumbers(List0, List0);
            SingleLinkedList<uint> test2 = Questions.AddListsAsNumbers(List1, List3);
            SingleLinkedList<uint> test3 = Questions.AddListsAsNumbers(List2, List4);
            SingleLinkedList<uint> test4 = Questions.AddListsAsNumbers(List3, List4);
            SingleLinkedList<uint> test5 = Questions.AddListsAsNumbers(List0, List4);
            SingleLinkedList<uint> test6 = Questions.AddListsAsNumbers(List6, List6);

            Assert.IsTrue(test1.ToArray().Compare(array1));
            Assert.IsTrue(test2.ToArray().Compare(array1));
            Assert.IsTrue(test3.ToArray().Compare(array6));
            Assert.IsTrue(test4.ToArray().Compare(array7));
            Assert.IsTrue(test5.ToArray().Compare(array5));
            Assert.IsTrue(test6.ToArray().Compare(array8));

        }

        ///<summary>
        ///A test method for SetDiagonalZero
        ///</summary>
        [TestMethod()]
        public void MatrixSetDiagonalZeroTest()
        {
            int rows = 10;
            int columns = 15;

            int[,] matrix = new int[rows, columns];

            Random rand = new Random();

            for(int x = 0; x < rows; x++)
            {
                for(int y = 0; y < columns; y++)
                {
                    int cellValue = rand.Next(0, 9);

                    if (cellValue == 0)
                    {
                        cellValue = rand.Next(0, 9) % 2;
                    }
                    matrix[x, y] = cellValue;
                }
            }

            int[,] result = matrix.Clone<int>();
            Questions.SetDiagonalZero(result);

            var matrixString = matrix.ToString<int>();
            var resultString = result.ToString<int>();
        }

        ///<summary>
        ///A test method for matrix rotation test
        ///</summary>
        [TestMethod()]
        public void RotationTest()
        {
            string[, ] matrix0 = new string[, ]
                {
                    {"a", "b", "c", "d", "e"},
                    {"g", "h", "i", "j", "k"},
                    {"l", "m", "n", "o", "p"},
                };
            string[, ] rotate1 = new string[, ] // matrix0 clockwise rotated 90-degree
                {
                    {"l", "g", "a"},
                    {"m", "h", "b"},
                    {"n", "i", "c"},
                    {"o", "j", "d"},
                    {"p", "k", "e"},
                };
            string[, ] rotate2 = new string[, ] // matrix0 clockwise rotated 180-degree, or flip vertically then horizontally
                {
                    {"p", "o", "n", "m", "l"},
                    {"k", "j", "i", "h", "g"},
                    {"e", "d", "c", "b", "a"},
                };
            string[, ] rotate3 = new string[, ] // matrix0 clockwise rotated 270-degree
                {
                    {"e", "k", "p"},
                    {"d", "j", "o"},
                    {"c", "i", "n"},
                    {"b", "h", "m"},
                    {"a", "g", "l"},
                };

            string[, ] matrix1 = Questions.GetMatrixRotation(matrix0, 90);
            string[, ] matrix2 = Questions.GetMatrixRotation(matrix0, 180);
            string[, ] matrix3 = Questions.GetMatrixRotation(matrix0, 270);

            Assert.IsTrue(matrix1.Compare(rotate1));
            Assert.IsTrue(matrix2.Compare(rotate2));
            Assert.IsTrue(matrix3.Compare(rotate3));

        }

        #endregion
    }
}
