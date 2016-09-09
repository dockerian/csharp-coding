using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Extensions;
using Common.Library;


namespace UnitTest
{
    ///<summary>
    ///This is a test class for CommonTest and is intended
    ///to contain all CommonTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommonTest
    {
        #region :: Private Fields

        private byte[] reverseLookup = new byte[]
            {
                0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, // 0x00 - 0x08
                0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
                0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, // 0x10 - 0x18
                0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
                0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, // 0x20 - 0x28
                0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
                0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, // 0x30 - 0x38
                0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
                0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, // 0x40 - 0x48
                0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
                0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, // 0x50 - 0x58
                0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA, 
                0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, // 0x60 - 0x68
                0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
                0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, // 0x70 - 0x78
                0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE, 
                0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, // 0x80 - 0x88
                0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
                0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, // 0x90 - 0x98
                0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,  
                0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, // 0xA0 - 0xA8
                0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5, 
                0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, // 0xB0 - 0xB8
                0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD, 
                0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, // 0xC0 - 0xC8
                0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 
                0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, // 0xD0 - 0xD8
                0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,  
                0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, // 0xE0 - 0xE8
                0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7, 
                0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, // 0xF0 - 0xF8
                0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
            };

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

        #region :: Tests :: CommonExtensions

        ///<summary>
        ///A test for Common Extensions
        ///</summary>
        [TestMethod()]
        public void CommonExtensionsTest()
        {
            string sysDirPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            DirectoryInfo dir = new DirectoryInfo(sysDirPath);
            XElement xElement = dir.ToXElement();
            Assert.IsNotNull(xElement);

            DirectoryInfo ced = new DirectoryInfo(Environment.CurrentDirectory);
            Assert.IsNotNull(ced.ToXElement());

            string bogusUrl = "https://mytest.mydomain.com.test";
            string errorUrl = "https://map.google.com/";
            string validUrl = "http://maps.google.com";

            Assert.IsFalse("".IsUrlAccessible());
            Assert.IsFalse(errorUrl.IsUrlAccessible());
            Assert.IsFalse(bogusUrl.IsUrlAccessible());
            Assert.IsFalse(bogusUrl.IsUrlValid());
            Assert.IsTrue(validUrl.IsUrlAccessible());
            Assert.IsTrue(validUrl.IsUrlValid());

        }

        ///<summary>
        ///A test for Common Extensions
        ///</summary>
        [TestMethod()]
        public void CommonExtensionsMatrixTest()
        {
            #region Array/Matrix Test Data

            string[, ] matrix0 = new string[, ]
                {
                    {"a", "b", "c", "d", "e"},
                    {"g", "h", "i", "j", "k"},
                };
            string[, ] matrix1 = new string[, ] // matrix0 clockwise rotated 90-degree
                {
                    {"g", "a"},
                    {"h", "b"},
                    {"i", "c"},
                    {"j", "d"},
                    {"k", "e"},
                };
            string[, ] matrix2 = new string[, ] // matrix0 clockwise rotated 180-degree, or flip vertically then horizontally
                {
                    {"k", "j", "i", "h", "g"},
                    {"e", "d", "c", "b", "a"},
                };
            string[, ] matrix3 = new string[, ] // matrix0 counter-clockwise rotated
                {
                    {"e", "k"},
                    {"d", "j"},
                    {"c", "i"},
                    {"b", "h"},
                    {"a", "g"},
                };
            string[, ] matrix4 = new string[, ] // matrix0 flip horizontally
                {
                    {"e", "d", "c", "b", "a"},
                    {"k", "j", "i", "h", "g"},
                };
            string[, ] matrix5 = new string[, ] // matrix1 flip horizontally or matrix3 flip vertically
                {
                    {"a", "g"},
                    {"b", "h"},
                    {"c", "i"},
                    {"d", "j"},
                    {"e", "k"},
                };
            string[, ] matrix6 = new string[, ] // matrix2 flip horizontally
                {
                    {"g", "h", "i", "j", "k"},
                    {"a", "b", "c", "d", "e"},
                };

            #endregion

            #region 2-D Array Test

            string[, ] result1 = matrix0.RotateClockwise();
            string[, ] result2 = matrix0.RotateClockwise180();
            string[, ] result3 = matrix0.RotateCounterClockwise();
            string[, ] result4 = matrix0.FlipHorizontally();
            string[, ] result5 = matrix3.FlipVertically();
            string[, ] result6 = matrix2.FlipHorizontally();

            Assert.IsFalse(result1.Compare(matrix0));
            Assert.IsFalse(result2.Compare(matrix0));
            Assert.IsFalse(result3.Compare(matrix0));
            Assert.IsFalse(result4.Compare(matrix0));
            Assert.IsFalse(result5.Compare(matrix0));
            Assert.IsFalse(result6.Compare(matrix0));

            Assert.IsTrue(result1.Compare(matrix1));
            Assert.IsTrue(result1.Compare(matrix1.Clone<string>()));
            Assert.IsTrue(result2.Compare(matrix2));
            Assert.IsTrue(result3.Compare(matrix3));
            Assert.IsTrue(result4.Compare(matrix4));
            Assert.IsTrue(result5.Compare(matrix5));
            Assert.IsTrue(result5.Compare(matrix3.FlipVertically()));
            Assert.IsTrue(result5.Compare(matrix0.RotateClockwise().FlipHorizontally()));
            Assert.IsTrue(result6.Compare(matrix6));

            object[, ] matrix7 = new object[, ]
                {
                    {"a", "b", "c", "d", "e"},
                    {"g", "h", "i", "j", "k"},
                };
            object[, ] result7 = new object[, ]
                {
                    {"a", "b", 102, "d", "e"},
                    {"g", "h", "i", "j", "k"},
                };

            Assert.IsFalse(result7.Compare(matrix7));

            #endregion

            #region Matrix Test

            Matrix<string> objectMatrix = new Matrix<string>(matrix0);
            Matrix<string> clonedMatrix = objectMatrix.Clone();
            Assert.IsTrue(objectMatrix.Compare(clonedMatrix));

            Assert.IsTrue(result1.Compare(objectMatrix.RotateClockwise()));
            Assert.IsTrue(result2.Compare(objectMatrix.RotateClockwise180()));
            Assert.IsTrue(result3.Compare(objectMatrix.RotateCounterClockwise()));
            Assert.IsTrue(result4.Compare(objectMatrix.FlipHorizontally()));
            Assert.IsTrue(result5.Compare(objectMatrix.RotateCounterClockwise().FlipVertically()));
            Assert.IsTrue(result6.Compare(objectMatrix.FlipVertically()));

            for(int x = 0; x < objectMatrix.Rows; x++)
            {
                for(int y = 0; y < objectMatrix.Columns; y++)
                {
                    Assert.IsTrue(objectMatrix[x, y] == clonedMatrix[x, y]);
                }
            }

            object[, ] matrix8 = new object[, ]
                {
                    {"00", "01", "02", "03", "04"},
                    {"10", "11", "12", "13", "14"},
                    {"20", "21", "22", "23", "24"},
                    {"30", "31", "32", "33", "34"},
                    {"40", "41", "42", "43", "44"},
                };
            object[, ] result8 = new object[, ]
                {
                    {"00", "10", "20", "30", "40"},
                    {"01", "11", "21", "31", "41"},
                    {"02", "12", "22", "32", "42"},
                    {"03", "13", "23", "33", "43"},
                    {"04", "14", "24", "34", "44"},
                };

            matrix8.RotateInline(MatrixRotationType.Clockwise);
            Assert.IsFalse(result8.Compare(matrix8));
            Assert.IsTrue(result8.Compare(matrix8.FlipHorizontally()));

            matrix8.RotateInline(MatrixRotationType.Clockwise180);
            Assert.IsFalse(result8.Compare(matrix8));

            matrix8.RotateInline(MatrixRotationType.Clockwise);
            matrix8.RotateInline(MatrixRotationType.CounterClockwise);
            Assert.IsTrue(result8.Compare(matrix8.FlipVertically()));

            #endregion

        }

        ///<summary>
        ///A test for DateTime Common Extensions
        ///</summary>
        [TestMethod()]
        public void CommonExtensionsDateTimeTest()
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

            var angle0 = dt0.GetAngle(); //   0.0
            var angle1 = dt1.GetAngle(); //  30.0
            var angle2 = dt2.GetAngle(); //   0.0
            var angle3 = dt3.GetAngle(); //  90.0
            var angle4 = dt4.GetAngle(); // 120.0
            var angle5 = dt5.GetAngle(); // 150.0
            var angle6 = dt6.GetAngle(); // 180.0
            var angle7 = dt7.GetAngle(); // 210.0
            var angle8 = dt8.GetAngle(); // 240.0
            var angle9 = dt9.GetAngle(); // 270.0

            Assert.AreEqual(dt0.GetAngleOfHourHand(), dt0.GetAngleOfMinuteHand());
            Assert.AreEqual(angle0, 0.0);
            Assert.AreEqual(angle1, 30.0);
            Assert.AreEqual(angle2, 0.0);
            Assert.AreEqual(angle3, 90.0);
            Assert.AreEqual(angle6, 180.0);
            Assert.AreEqual(angle9, 270.0);

            int counterFound;
            KeyValuePair<DateTime, double>[] results = Generic.GetTimeByAngle(angle3, out counterFound);
            Assert.AreEqual(counterFound, 22);

        }

        #endregion

        #region :: Tests :: Algorithm

        ///<summary>
        ///A test for Common.Abs()
        ///</summary>
        [TestMethod()]
        public void AbsTest()
        {
            int v = System.Int32.MinValue;
            int expected = 0; // expecting overflow exception
            int actual;

            try
            {
                actual = Generic.Abs(v);
            }
            catch (System.Exception ex)
            {
                Assert.IsInstanceOfType(ex, System.Type.GetType("System.OverflowException"));
            }

            v = System.Int32.MinValue + 1;
            expected = System.Int32.MaxValue;
            actual = Generic.Abs(v);
            Assert.AreEqual(expected, actual);

        }

        ///<summary>
        ///A test for Common.Abs()
        ///</summary>
        [TestMethod()]
        public void CompareToTest()
        {
            float fv1 = 3.0f / 5.0f;
            float fv2 = 9.001f / 15.01f;
            float fv3 = 9.00000001f / 15.0000001f;

            double dv1 = 3.0 / 5.0;
            double dv2 = 9.001 / 15.01;
            double dv3 = 9.00000001 / 15.00000001;

            Assert.IsFalse(Generic.CompareAbs(fv1, fv2));
            Assert.IsFalse(Generic.CompareAbs(fv1, -fv2));
            Assert.IsFalse(Generic.CompareAbs(-fv1, fv2));
            Assert.IsFalse(Generic.CompareAbs(-fv1, -fv2));
            Assert.IsFalse(Generic.CompareAbs(dv1, dv2));
            Assert.IsFalse(Generic.CompareAbs(dv1, -dv2));
            Assert.IsFalse(Generic.CompareAbs(-dv1, dv2));
            Assert.IsFalse(Generic.CompareAbs(-dv1, -dv2));

            Assert.IsTrue(Generic.CompareAbs(fv1, fv3));
            Assert.IsTrue(Generic.CompareAbs(fv1, -fv3));
            Assert.IsTrue(Generic.CompareAbs(-fv1, fv3));
            Assert.IsTrue(Generic.CompareAbs(-fv1, -fv3));
            Assert.IsTrue(Generic.CompareAbs(dv1, dv3));
            Assert.IsTrue(Generic.CompareAbs(dv1, -dv3));
            Assert.IsTrue(Generic.CompareAbs(-dv1, dv3));
            Assert.IsTrue(Generic.CompareAbs(-dv1, -dv3));

            Assert.AreEqual(fv1.Compare(fv2), 1);
            Assert.AreEqual(fv1.Compare(-fv2), 1);
            Assert.AreEqual((-fv1).Compare(fv2), -1);
            Assert.AreEqual((-fv1).Compare(-fv2), -1);
            Assert.AreEqual(fv2.Compare(fv1), -1);
            Assert.AreEqual(fv2.Compare(-fv1), 1);
            Assert.AreEqual((-fv2).Compare(-fv1), 1);
            Assert.AreEqual((-fv2).Compare(fv1), -1);
            Assert.AreEqual(fv1.Compare(fv3), 0);

            Assert.AreEqual(dv1.Compare(dv2), 1);
            Assert.AreEqual(dv1.Compare(-dv2), 1);
            Assert.AreEqual((-dv1).Compare(dv2), -1);
            Assert.AreEqual((-dv1).Compare(-dv2), -1);
            Assert.AreEqual(dv2.Compare(dv1), -1);
            Assert.AreEqual(dv2.Compare(-dv1), 1);
            Assert.AreEqual((-dv2).Compare(-dv1), 1);
            Assert.AreEqual((-dv2).Compare(dv1), -1);
            Assert.AreEqual(dv1.Compare(dv3), 0);

        }

        ///<summary>
        ///A test for Common.IsPowerOfTwo()
        ///</summary>
        [TestMethod()]
        public void IsPowerOfTwoTest()
        {
            int v = System.Int32.MinValue;
            Assert.AreEqual(false, Generic.IsPowerOfTwo(v));
            v = System.Int32.MinValue + 1;
            Assert.AreEqual(false, Generic.IsPowerOfTwo(v));
            v = System.Int32.MaxValue;
            Assert.AreEqual(false, Generic.IsPowerOfTwo(v));
        }

        ///<summary>
        ///A test for Common.IsPrime()
        ///</summary>
        [TestMethod()]
        public void IsPrimeTest()
        {
            var range = 1000; // Int32.MaxValue
            for(int number = -10; number <= range; number++)
            {
                bool test1 = number.IsPrime();
                bool test2 = Generic.IsPrimeByLinq(number);
              //bool test3 = Generic.IsPrimeBySieveOfEratosthenes(number);
                bool test4 = Generic.IsPrime(number);
                Assert.AreEqual(test1, test2);
              //Assert.AreEqual(test1, test3);
                Assert.AreEqual(test1, test4);
            }
        }

        #endregion

        #region :: Tests :: String

        ///<summary>
        ///A test for ReverseBits
        ///</summary>
        [TestMethod()]
        public void ReverseBufferTest()
        {
            byte[] buffer = new byte[] { 0xF0, 0x55, 0xD2, 0x31, 0xE1 };
            byte[] expected = new byte[]{ 0x87, 0x8C, 0x4B, 0xAA, 0x0F };
            byte[] actual;

            actual = Generic.ReverseBufferBits(buffer);

            for(int i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(
                    this.reverseLookup[buffer[i]],
                    expected[buffer.Length -1-i] 
                    );
            }
        }

        ///<summary>
        ///A test for ReverseBit
        ///</summary>
        [TestMethod()]
        public void ReverseBitTest()
        {
            byte bits = 0x57;
            byte expected = reverseLookup[(int)bits];
            byte actual_1 = Generic.ReverseBits(bits); // result from the function to be tested
            byte actual_2 = Generic.ReverseBitsBy32bit(bits); // result from the function to be tested
            byte actual_3 = Generic.ReverseBitsBy32bitProcessor(bits); // result from the function to be tested
            byte actual_4 = Generic.ReverseBitsBy64bitProcessor(bits); // result from the function to be tested
            byte actual_5 = Generic.ReverseBitsByLookup(bits); // result from the function to be tested
            byte actual_6 = Generic.ReverseBitsByLookup4x4(bits); // result from the function to be tested
            byte actual_7 = Generic.ReverseBitsByMask(bits); // result from the function to be tested

            Assert.AreEqual(expected, actual_1);
            Assert.AreEqual(expected, actual_2);

            if (OS.Is64Bit || Environment.Is64BitProcess)
            {
                Assert.AreEqual(expected, actual_4);
            }
            else // non 64-bit processor (32-bit or unknown)
            {
                Assert.AreEqual(expected, actual_3);
            }
            Assert.AreEqual(expected, actual_5);
            Assert.AreEqual(expected, actual_6);
            Assert.AreEqual(expected, actual_7);

        }

        [TestMethod()]
        public void ReverseBitTestAllData()
        {
            // using all bytes from the lookup table
            for(int i = 0; i < this.reverseLookup.Length; i++)
            {
                byte bits = this.reverseLookup[i];
                byte data = reverseLookup[(int)bits]; // expected result
                byte test = Generic.ReverseBits(bits); // result from the function to be tested

                Assert.AreEqual(test, data);
            }

        }

        [TestMethod()]
        public void ReverseStringTest()
        {
            string str_null = null;
            string strBlank = "  ";
            string strEmpty = String.Empty;
            string s_result = "";

            Assert.IsNull(str_null.Reverse());
            Assert.AreEqual(strEmpty.Reverse(), String.Empty);
            Assert.AreEqual(strEmpty.Reverse(), strEmpty);

            strBlank = "  \t";
            s_result = "\t  ";
            Assert.AreEqual(strBlank.Reverse(), s_result);

            string expected = "gfedcba";
            string a_string = "abcdefg";
            string a_result = a_string.Reverse();

            Assert.AreEqual(a_result, expected);
            Assert.AreEqual(a_string.Reverse(), expected);
            Assert.AreEqual(expected.Reverse(), a_string);

            a_string = "äëïöü";
            expected = "üöïëä";
            Assert.AreEqual(a_string.Reverse(true), expected);
            Assert.AreEqual(a_string.Reverse(), expected);

            a_string = "-=-測試.繁體中文字-=-";
            expected = "-=-字文中體繁.試測-=-";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "风清醉明月，径花入影斜。空夜怀思静，梦飞双栖蝶";
            expected = "蝶栖双飞梦，静思怀夜空。斜影入花径，月明醉清风";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "扬歌轻舟藏处远，影红缀流映青天。香荷沃野遍翠绿，翔鸭戏水荡风闲";
            expected = "闲风荡水戏鸭翔，绿翠遍野沃荷香。天青映流缀红影，远处藏舟轻歌扬";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "水活魚戲魚活水，春念花嬌花念春";
            expected = "春念花嬌花念春，水活魚戲魚活水";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "柳边桥入桥边柳，烟里村生村里烟。笔作花时花作笔，弦无妙处妙无弦";
            expected = "弦无妙处妙无弦，笔作花时花作笔。烟里村生村里烟，柳边桥入桥边柳";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "香暗绕窗纱，半帘疏影遮。霜枝一挺干，玉树几开花。傍水笼烟薄，隙墙穿月斜。芳梅喜淡雅，永日伴清茶";
            expected = "茶清伴日永，雅淡喜梅芳。斜月穿墙隙，薄烟笼水傍。花开几树玉，干挺一枝霜。遮影疏帘半，纱窗绕暗香";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "开篷一棹远溪流，走上烟花踏径游。来客仙亭闲伴鹤，泛舟渔浦满飞鸥。台映碧泉寒井冷，月明孤寺古林幽。回望四山观落日，偎林傍水绿悠悠";
            expected = "悠悠绿水傍林偎，日落观山四望回。幽林古寺孤明月，冷井寒泉碧映台。鸥飞满浦渔舟泛，鹤伴闲亭仙客来。游径踏花烟上走，流溪远棹一篷开";
            Assert.AreEqual(a_string.Reverse(true), expected);

            //宋代李禺回文诗《两相思》：
            a_string = "枯眼望遥山隔水，往来曾见几心知。壶空怕酌一杯酒，笔下难成和韵诗。途路阳人离别久，讯音无雁寄回迟。孤灯夜守长寥寂，夫忆妻兮父忆儿";
            expected = "儿忆父兮妻忆夫，寂寥长守夜灯孤。迟回寄雁无音讯，久别离人阳路途。诗韵和成难下笔，酒杯一酌怕空壶。知心几见曾来往，水隔山遥望眼枯";
            Assert.AreEqual(a_string.Reverse(true), expected);

            a_string = "a b c 1 2 3 4 5 6 7 8 9 0 defghijklmnopqrstuvwxyz ...";
            expected = a_string.Reverse();
            Assert.AreNotEqual(a_string, expected);
            Assert.AreNotEqual(a_string.ReverseByXOR(), a_string);
            Assert.AreEqual(a_string.Reverse().Reverse(), a_string);
            Assert.AreNotEqual(a_string.Reverse(), a_string);

            a_string = "this is test; that is 'my book'.";
            a_result = a_string.ReverseWord();
            expected = "'my book' is that; test is this.";//can the algorithm really parse this?
            Assert.AreNotEqual(a_result, expected);//not at this moment ...

            a_string = "this is rosetta's test; that is 'my book'.";
            expected = "'my book' is that; rosetta's test is this.";//can the algorithm really parse this?
            Assert.AreNotEqual(a_string.ReverseWord(), expected);//not at this moment ...

            a_string = "mine is yours";
            expected = "yours is mine";
            Assert.AreEqual(a_string.ReverseWord(), expected);

        }

        [TestMethod()]
        public void StringTokenTest()
        {
            foreach(var s in StringToken.ClosingTokens)
            {
                if (s.Length == 1)
                {
                    Char ch = s[0];
                    Assert.AreEqual(ch.IsClosingToken(), true);
                }
                Assert.AreEqual(StringToken.IsClosingToken(s), true);
                Assert.AreEqual(s.IsClosingToken(), true);
            }
            foreach(var s in StringToken.OpeningTokens)
            {
                if (s.Length == 1)
                {
                    Char ch = s[0];
                    Assert.AreEqual(ch.IsOpeningToken(), true);
                }
                Assert.AreEqual(StringToken.IsOpeningToken(s), true);
                Assert.AreEqual(s.IsOpeningToken(), true);
            }

        }

        [TestMethod()]
        public void SubStringTest()
        {
            string a = null;
            string b = null;
            string s = null;

            Assert.IsFalse(a.IsSubstring(b));
            Assert.IsFalse(b.IsSubstring(a));
            Assert.IsFalse(b.IsSubstring(b));

            b = "bbbb";
            Assert.IsFalse(a.IsSubstring(b));
            Assert.IsFalse(b.IsSubstring(a));

            a = "aaaa";
            Assert.IsFalse(a.IsSubstring(b));
            Assert.IsFalse(b.IsSubstring(a));

            a = "";
            Assert.IsFalse(a.IsSubstring(b));

            a = "xxxx";
            b = "----xxxx";
            Assert.IsTrue(a.IsSubstring(b));

            s = "xxxx----";
            Assert.IsTrue(a.IsSubstring(s));
            s = "----汉字xxxx国标码----";
            Assert.IsTrue(a.IsSubstring(s));

            a = "汉字";
            Assert.IsTrue(a.IsSubstring(s));
            a = "字xxxx国";
            Assert.IsTrue(a.IsSubstring(s));

            a = "--漢字xx";
            Assert.IsFalse(a.IsSubstring(s));

            a = "xxxx國标码-";
            Assert.IsFalse(a.IsSubstring(s));

            a = "xx国标码xx";
            Assert.IsFalse(a.IsSubstring(s));

            s = "xx国标码";
            Assert.IsFalse(a.IsSubstring(s));

            //TODO: need to find a test case with unicode
            a = "\u70B9\u83DC";
            s = "----\u70B9\u83DC----";
            Assert.IsTrue(a.IsSubstring(s));
        }

        #endregion

        #endregion
    }
}
