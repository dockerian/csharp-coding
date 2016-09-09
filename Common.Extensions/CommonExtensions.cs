using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;


namespace Common.Extensions
{
    public static class CommonExtensions
    {
        public const int MAX_DIRECTORY_LEVEL = 128;

        #region Algorithm

        public static int Compare(this double a, double b, double epsilon = 0.0000001)
        {
            if (a >= 0.0 && b < 0.0) return 1;
            if (b >= 0.0 && a < 0.0) return -1;

            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(absA - absB);

            int retSign = a < 0.0 && b < 0.0 ? - 1 : 1;
            bool isSame = diff < epsilon;

            return isSame ? 0 : retSign * (absA > absB ? 1 : -1);
        }
        public static int Compare(this float a, float b, float epsilon = 0.0000001f)
        {
            if (a >= 0.0 && b < 0.0) return 1;
            if (b >= 0.0 && a < 0.0) return -1;

            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(absA - absB);

            int retSign = a < 0.0 && b < 0.0 ? -1 : 1;
            bool isSame = diff < epsilon;

            return isSame ? 0 : retSign * (absA > absB ? 1 : -1);
        }

        public static bool IsPrime(this int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;

            for(int i = 2; i < number; ++i)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static double Mod(this double doubleValue, double division)
        {
            int times = (int)(doubleValue / division);

            return doubleValue - division * times;
        }

        public static float Mod(this float doubleValue, float division)
        {
            int times = (int)(doubleValue / division);

            return doubleValue - division * times;
        }

        /// <summary>
        /// Newton Raphson method for Square Root
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="precision">precision</param>
        /// <returns>square root of number</returns>
        public static double SquareRoot(this double number, double precision)
        {
            double root = number / 2.0;

            if (Math.Abs(root * root - number) <= precision)
            {
                return root;
            }
            while(Math.Abs(root * root - number) > precision)
            {
                root = root - (root * root - number) / (2 * root);
            }
            return root;
        }

        #endregion

        #region Array and List

        public static T[][] Clone<T>(this T[][] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0][];

            int rows = matrix.Length;

            T[][] clone = new T[rows][];

            for(var x = 0; x < rows; x++)
            {
                int columns = matrix[x].Length;

                for(var y = 0; y < columns; y++)
                {
                    clone[x][y] = matrix[x][y];
                }
            }

            return clone;
        }

        public static T[,] Clone<T>(this T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0,0];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] clone = new T[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    clone[x, y] = matrix[x, y];
                }
            }

            return clone;
        }

        public static bool Compare<T>(this T[] array1, T[] array2)
        {
            if (array1 == null || array2 == null) return false;

            int count1 = array1.Length;
            int count2 = array2.Length;

            if (count1 != count2) return false;

            bool hasSameElement = true;

            for(int i = 0; i < count1; i++)
            {
                if (array1[i] == null && array2[i] == null)
                {
                    continue; // null at the same spot
                }
                else if (array1[i] == null || array2[i] == null)
                {
                    hasSameElement = false;
                    break;
                }
                if (array1[i].GetType() != array2[i].GetType())
                {
                    hasSameElement = false;
                    break;
                }
                IComparable v1 = array1[i] as IComparable;
                IComparable v2 = array2[i] as IComparable;

                if (v1 == null || v2 == null)
                {
                    if (!array1[i].Equals(array2[i]))
                    {
                        hasSameElement = false;
                        break;
                    }
                }
                else if (v1.CompareTo(v2) != 0)
                {
                    hasSameElement = false;
                    break;
                }
            }
            return hasSameElement;
        }

        public static bool Compare<T>(this T[,] matrix1, T[,] matrix2)
        {
            if (matrix1 == null || matrix2 == null) return false;

            int rows1 = matrix1.GetLength(0);
            int columns1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int columns2 = matrix2.GetLength(1);

            if (rows1 != rows2 || columns1 != columns2) return false;

            for(var x = 0; x < rows1; x++)
            {
                for(var y = 0; y < columns1; y++)
                {
                    if (matrix1[x, y] == null && matrix2[x, y] == null)
                    {
                        continue; // null at the same spot
                    }
                    else if (matrix1[x, y] == null || matrix2[x, y] == null)
                    {
                        return false;
                    }
                    if (matrix1[x, y].GetType() != matrix2[x, y].GetType())
                    {
                        return false;
                    }
                    IComparable v1 = matrix1[x, y] as IComparable;
                    IComparable v2 = matrix2[x, y] as IComparable;

                    if (v1 == null || v2 == null)
                    {
                        if (!matrix1[x, y].Equals(matrix2[x, y]))
                        {
                            return false;
                        }
                    }
                    else if (v1.CompareTo(v2) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int[] IncreaseBy(this int[] a, int by = 1)
        {
            bool increment = @by >= 0;
            int bSize = 1;
            int byAbs = Math.Abs(@by);
            string bs = byAbs.ToString(CultureInfo.InvariantCulture);
            string ss = String.Empty;

            for(int d = @by/10; d > 0; bSize++) d /= 10; // get size of by

            int carry = 0;
            List<int> r = new List<int>();

            for(int i = 0, byVal = @by; i < a.Length || i < bSize; i++)
            {
                int b = byVal%10;
                byVal = byVal/10;
                int s = b + (i < a.Length ? a[i] : 0) + carry;
                if (s < 0)
                {
                    s += 10; carry = -1;
                }
                else if (s > 10)
                {
                    s %= 10; carry = 1;
                }
                else // clearing carry bit
                {
                    carry = 0;
                }
                if (i < a.Length)
                {
                    a[i] = s;
                }
                ss = s.ToString(CultureInfo.InvariantCulture) + ss;
                r.Add(s);
            }

            if (carry < 0)
            {
                // clear int[] since it does not support negative number
                for(int i = 0; i < a.Length; i++) a[i] = 0;
                return new int[1] { 0 };
            }
            else if (carry > 0)
            {
                r.Add(carry);
            }

            return a = r.ToArray();

        }

        public static bool IsSorted<T>(this T[] array, bool byDescending = false) where T : IComparable
        {
            if (array == null) return false;
            if (array.Length < 2) return true;

            for(int i = 1; i < array.Length; i++)
            {
                int compValue = array[i - 1].CompareTo(array[i]);

                if (compValue > 0 && !byDescending || byDescending && compValue < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsSorted<T>(this List<T> aList, bool byDescending = false) where T : IComparable
        {
            if (aList == null) return false;
            if (aList.Count < 2) return true;

            for(int i = 1; i < aList.Count; i++)
            {
                int compValue = aList[i - 1].CompareTo(aList[i]);

                if (compValue > 0 && !byDescending || byDescending && compValue < 0)
                {
                    return false;
                }
            }
            return true;
        }

        #region Array :: Flip and Rotation

        public static T[,] FlipHorizontally<T>(this T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0, 0];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] rotation = new T[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    rotation[x, y] = matrix[x, columns - y - 1];
                }
            }

            return rotation;
        }

        public static T[,] FlipVertically<T>(this T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0, 0];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] rotation = new T[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    rotation[x, y] = matrix[rows - x - 1, y];
                }
            }

            return rotation;
        }

        public static T[,] Rotate<T>(this T[,] matrix, MatrixRotationType rotationType = MatrixRotationType.Clockwise)
        {
            T[,] rotation = null;

            switch(rotationType)
            {
                case MatrixRotationType.Clockwise:
                {
                    rotation = matrix.RotateClockwise();
                    break;
                }
                case MatrixRotationType.Clockwise180:
                {
                    rotation = matrix.RotateClockwise180();
                    break;
                }
                case MatrixRotationType.CounterClockwise:
                {
                    rotation = matrix.RotateCounterClockwise();
                    break;
                }
            }

            return rotation;
        }

        public static T[,] RotateClockwise<T>(this T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0, 0];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] rotation = new T[columns, rows];

            for(var y = 0; y < columns; y++)
            {
                for(var x = 0; x < rows; x++)
                {
                    rotation[y, x] = matrix[rows - x - 1, y];
                }
            }

            return rotation;
        }

        public static T[,] RotateClockwise180<T>(this T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0, 0];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] rotation = new T[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    rotation[x, y] = matrix[rows - x - 1, columns - y - 1];
                }
            }

            return rotation;
        }

        public static T[,] RotateCounterClockwise<T>(this T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] rotation = new T[columns, rows];

            for(var y = 0; y < columns; y++)
            {
                for(var x = 0; x < rows; x++)
                {
                    rotation[y, x] = matrix[x, columns - y - 1];
                }
            }

            return rotation;
        }

        public static bool RotateInline<T>(this T[,] matrix, MatrixRotationType rotationType = MatrixRotationType.Clockwise)
        {
            if (matrix == null || matrix.Length < 1) return false;

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            int round = rows / 2 + 1;

            if (rows != columns) return false;

            if (round == 1) return true;

            for(int x = 0; x <= round; x++)
            {
                int range = round - x;

                for(int y = x; y <= range; y++)
                {
                    int r = rows - x - 1; // opposite in row
                    int c = columns - y - 1; // opposite in column

                    var t = matrix[x, y]; // keep start point

                    switch (rotationType)
                    {
                        case MatrixRotationType.Clockwise:
                        {
                            matrix[x, y] = matrix[c, x];
                            matrix[c, x] = matrix[r, c];
                            matrix[r, c] = matrix[y, r];
                            matrix[y, r] = t;

                            break;
                        }
                        case MatrixRotationType.Clockwise180:
                        {
                            matrix[x, y] = matrix[r, c]; // switch
                            matrix[r, c] = t;

                            var s = matrix[c, x]; // keep switching point
                            matrix[c, x] = matrix[y, r];
                            matrix[y, r] = s;

                            break;
                        }
                        case MatrixRotationType.CounterClockwise:
                        {
                            matrix[x, y] = matrix[y, r];
                            matrix[y, r] = matrix[r, c];
                            matrix[r, c] = matrix[c, x];
                            matrix[c, x] = t;

                            break;
                        }
                    }
                }
            }
            return true;
        }

        public static String ToString<T>(this T[, ] matrix)
        {
            if (matrix == null) return null;

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            string result = String.Empty;

            for(int x = 0; x < rows; x++)
            {
                for(int y = 0; y < columns; y++)
                {
                    string elementValue = (matrix[x, y] != null) ? matrix[x, y].ToString() : "null";

                    result += elementValue + (y < columns - 1 ? ", " : "");
                }
                result += "\r\n";
            }
            return result;
        }

        #endregion

        #endregion

        #region DirectoryInfo

        public static XElement ToXElement(this DirectoryInfo dir, int startLevel = 0)
        {
            if (dir == null) return null;

            XElement dirXElement = new XElement
                (
                dir.GetType().Name, 
                new XAttribute("FullName", dir.FullName),
                new XAttribute("Name", dir.Name)
                );

            try
            {
                foreach(DirectoryInfo subDir in dir.GetDirectories())
                {
                    var xElement = subDir.ToXElement();

                    if (xElement != null && startLevel < MAX_DIRECTORY_LEVEL)
                    {
                        dirXElement.Add(xElement, ++startLevel);
                    }
                }
                foreach(FileInfo file in dir.GetFiles())
                {
                    XElement fileXElement = new XElement
                        (
                        file.GetType().Name,
                        new XAttribute("Name", file.Name)
                        );
                    dirXElement.Add(fileXElement);
                }
            }
            catch (Exception ex)
            {
                string exceptionName = ex.GetType().FullName;
                dirXElement.SetElementValue("Exception", exceptionName);
            }


            return dirXElement;
        }

        #endregion

    }
}
