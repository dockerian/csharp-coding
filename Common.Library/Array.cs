using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Common.Library
{
    /// <summary>
    /// A class represents an arithmetic array that can apply basic arithmetic operations.
    /// </summary>
    public class ArithmeticArray : IComparable<ArithmeticArray>
    {
        #region fields
        private List<int> _array;
        private bool _isPositive = true;
        #endregion

        #region constructors

        /// <summary>
        /// Initialize a new instance of <see cref="ArithmeticArray" />.
        /// </summary>
        /// <param name="a">An arithmetic array.</param>
        /// <param name="keepSign">Indicates if to keep the sign.</param>
        public ArithmeticArray(ArithmeticArray a, bool keepSign = true)
        {
            if (a == null || a.Size == 0)
            {
                _array = new List<int>() {0};
                return;
            }
            else // initialization
            {
                _array = new List<int>();
                _isPositive = keepSign ? a.IsPositive : true;
            }
            for(int i = 0; i < a.Size; i++)
            {
                _array.Add(a[i]);
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ArithmeticArray" /> from a series of digits (assuming all positive numbers between 0..9)
        /// </summary>
        /// <param name="positive">number sign (positive or negative)</param>
        /// <param name="digits">array of digits</param>
        public ArithmeticArray(bool positive, params int[] digits)
        {
            if (digits.Length == 0)
            {
                _array = new List<int>() {0};
                return;
            }
            _array = new List<int>();
            _isPositive = positive;

            bool zerolead = true;

            for(int i = digits.Length - 1; i >= 0; i--)
            {
                int number = Math.Abs(digits[i]);

                if (zerolead && number != 0) zerolead = false;

                if (zerolead == false)
                {
                    string sn = number.ToString(CultureInfo.InvariantCulture);

                    for(int j = sn.Length - 1; j >= 0; j--)
                    {
                        int digit = sn[j] - '0';
                        _array.Insert(0, digit);
                    }
                }
            }
            if (_array.Count == 0)
            {
                _array.Add(0);
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ArithmeticArray" /> from an integer.
        /// </summary>
        /// <param name="digits">The base of an integer.</param>
        /// <param name="keepSign">Indicates to keep the sign.</param>
        /// <param name="powerX">The power to apply on the base integer value.</param>
        public ArithmeticArray(int digits, bool keepSign = true, int powerX = 0)
        {
            if (digits < 0)
            {
                digits = -digits; _isPositive = keepSign? false : true;
            }

            _array = new List<int>();

            int digit = 0;
            int dLeft = digits;

            do // filling up the array
            {
                digit = dLeft % 10;
                dLeft = dLeft / 10;
                _array.Add(digit);
            }
            while (dLeft != 0);

            for(int i = 0; i < powerX; i++)
            {
                _array.Insert(0, 0);
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ArithmeticArray" /> from a string.
        /// </summary>
        /// <param name="digits">A string of digits.</param>
        public ArithmeticArray(string digits)
        {
            bool accepted = false;
            bool zerolead = true;

            if (String.IsNullOrWhiteSpace(digits) == false)
            {
                digits = digits.Trim();
                _array = new List<int>();
                _isPositive = digits[0] != '-';
                accepted = true;

                for(int i = 0; i < digits.Length; i++)
                {
                    if (Char.IsDigit(digits[i]))
                    {
                        int digit = digits[i] - '0';
                        if (zerolead && digit != 0) zerolead = false;
                        if (zerolead == false)
                        {
                            _array.Insert(0, digit);
                        }
                    }
                    else if (i != digits.Length - 1 && (digits[i] != '-' && digits[i] != '+'))
                    {
                        accepted = false;
                        break;
                    }
                }
            }

            if (accepted == false)
            {
                _array = new List<int>() {0};
            }

            if (_array.Count == 0)
            {
                _array.Add(0);
            }
        }

        #endregion

        #region properties

        public int HighIndex
        {
            get { return this._array.Count - 1; }
        }

        public int HighIndexValue
        {
            get { return this._array.Last(); }
        }

        public bool IsPositive { get { return _isPositive; } }

        public bool IsZero
        {
            get { return this._array.Count == 1 && this._array[0] == 0; }
        }

        public int Size { get { return _array.Count; } }

        #endregion

        #region static functions
        /// <summary>
        /// Addition of two ArithmeticArray object
        /// </summary>
        /// <param name="a">the left operand</param>
        /// <param name="b">the right operand</param>
        /// <param name="addToLeftOperand">indicating change and return left operand</param>
        /// <returns>a new instance or changed left operand</returns>
        private static ArithmeticArray arithmeticAddition(ArithmeticArray a, ArithmeticArray b, bool addToLeftOperand = false)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return null;

            ArithmeticArray s;

            bool biggerValue = compare(a, b, true) >= 0;

            if (biggerValue)
            {
                s = addToLeftOperand ? a : new ArithmeticArray(a);
            }
            else // switch operands
            {
                if (addToLeftOperand)
                {
                    s = new ArithmeticArray(a);
                    a = changeValueByList(a, b._array);
                    b = s;
                    s = a;
                }
                else
                {
                    s = new ArithmeticArray(b);
                    b = a;
                }
            }

            int carryOn = 0;
            bool increment = s.IsPositive == b.IsPositive; // same sign

            if (increment)
            {
                for(int i = 0; i < s.Size; i++)
                {
                    int biv = i < b.Size ? b[i] : 0;
                    int sum = s[i] + biv + carryOn;
                    carryOn = (sum > 9) ? 1 : 0;
                    s[i] = sum % 10;
                }
                if (carryOn > 0)
                {
                    s._array.Add(carryOn);
                }
            }
            else // subtraction
            {
                for(int i = 0; i < s.Size; i++)
                {
                    int biv = (i < b.Size ? b[i] : 0) + carryOn;
                    int sub = (s[i] >= biv) ? (s[i] - biv) : (s[i] + 10 - biv);
                    carryOn = (s[i] >= biv) ? 0 : 1;
                    s[i] = sub;
                }
                for(int x = s.Size - 1; x >= 0; x--)
                {
                    if (s[x] != 0) break;
                    if (x != 0) s._array.RemoveAt(x);
                }
            }
            if (s.IsZero)
            {
                s._isPositive = true;
            }

            return addToLeftOperand ? new ArithmeticArray(s) : s;
        }
        /// <summary>
        /// Add integer value to ArithmeticArray object
        /// </summary>
        /// <param name="a">the left operand</param>
        /// <param name="by">increment/decrement</param>
        /// <param name="x">zero-based bit index of ArithmeticArray object (power of 10)</param>
        /// <returns></returns>
        private static ArithmeticArray arithmeticAddition(ArithmeticArray a, int by = 1, int x = 0, bool addToLeftOperand = false)
        {
            if (a == null || a.Size == 0) return null;

            if (by == 0)
            {
                return new ArithmeticArray(a);
            }

            if (x < 0) x = 0;

            bool biggerRightOprand = x >= a.Size || x == a.HighIndex && by > a.HighIndexValue;

            if (by < -9 || by > 9 || biggerRightOprand)
            {
                var b = new ArithmeticArray(by);
                for(int i = 0; i < x; i++) b._array.Insert(0, 0);
                return arithmeticAddition(a, b, addToLeftOperand);
            }

            /*// the case of x >= a.Size has been ruled out earlier; otherwise, fill up more with zero
            for(int j = a.Size; j <= x; j++) a._array.Add(0);
            //*/
            int hi_pos = a.Size - 1;
            int change = Math.Abs(by); // between 0..9

            bool increment = (by > 0 && a.IsPositive) || (by < 0 && a.IsPositive == false);

            if (increment)
            {
                for(int i = x; i < a.Size && change > 0; i++)
                {
                    int svalue = a._array[i] + change;
                    change = svalue > 9 ? 1 : 0;
                    a._array[i] = svalue % 10;
                }
                if (change > 0)
                {
                    a._array.Add(change);
                }
            }
            else // decrement
            {
                for(int i = x; i < a.Size; i++)
                {
                    if (a._array[i] >= change)
                    {
                        a._array[i] -= change; change = 0;
                    }
                    else if (i >= hi_pos)
                    {
                        a._array[i] = change - a._array[i];
                        a._isPositive = !a._isPositive;
                        change = 0;
                    }
                    else // borrowing from higher position
                    {
                        a._array[i] -= change - 10; change = 1;
                    }
                    if (a._array[i] == 0 && i == hi_pos)
                    {
                        if (i == 0)
                        {
                            a._isPositive = true;
                        }
                        else
                        {
                            a._array.RemoveAt(i);
                        }
                    }
                    if (change == 0)
                    {
                        break;
                    }
                }
            }

            return new ArithmeticArray(a);
        }

        private static ArithmeticArray arithmeticDivision(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return null;

            if (b.IsZero) throw new DivideByZeroException("ArithmeticaArray cannot divide by zero.");

            if (a.IsZero || compare(a, b, true) < 0)
            {
                return new ArithmeticArray(0); // a is zero or by ignoring sign a < b
            }
            if (b.HighIndex == 0 && b.HighIndexValue == 1) // b is 1 or b is -1
            {
                ArithmeticArray r = new ArithmeticArray(a);
                r._isPositive = a.IsPositive == b.IsPositive;
                return r;
            }

            ArithmeticArray d = new ArithmeticArray(0);
            ArithmeticArray s = new ArithmeticArray(a, false);
            ArithmeticArray g = new ArithmeticArray(b, false);

            int giv = g[g.Size - 1] * 10 + (g.Size > 1 ? g[g.Size - 2] : 0) + 1;

            while(s.Size > g.Size)
            {
                int siv = s[s.Size - 1] * 100 + (s.Size > 1 ? s[s.Size - 2] * 10 : 0);
                int div = siv / giv;
                int xiv = s.Size - g.Size - 1; // offset, must be >= 0

                if (div > 9)
                {
                    //div /= 10; xiv++;
                }
                if (div > 0)
                {
                    ArithmeticArray sub = arithmeticMultiplication(g, div, xiv);

                    if (s < sub) break;

                    ArithmeticArray dia = new ArithmeticArray(div, false, xiv);

                    d = d + dia;
                    s = s - sub;
                }
                else
                {
                    break;
                }
            }
            while(s >= g)
            {
                s = s - g;
                d++;
            }

            d._isPositive = d.IsZero || a._isPositive == b._isPositive;

            return d;
        }
        private static ArithmeticArray arithmeticDivision(ArithmeticArray a, int by = 1, int x = 0)
        {
            if (a == null || a.Size == 0) return null;

            if (by == 0) throw new DivideByZeroException("ArithmeticaArray cannot divide by zero.");

            ArithmeticArray d = new ArithmeticArray(0);

            if (a.IsZero) return d;

            if (by == 1 || by == -1)
            {
                d = new ArithmeticArray(a);
                for(int i = 0; i < x; i--) d._array.RemoveAt(0);
                d._isPositive = by >= 0 && a._isPositive || by < 0 && !a._isPositive;
                return d;
            }

            int biv = Math.Abs(by * (int)Math.Pow(10, x));
            string division = biv.ToString(CultureInfo.InvariantCulture);
            ArithmeticArray s = new ArithmeticArray(a, false); // s >= 0
            ArithmeticArray g = new ArithmeticArray(biv); // g >= 0

            int giv = 10 * (division[0] - '0') + (division.Length > 1 ? division[1] - '0' : 0) + 1;

            while(s.Size > division.Length)
            {
                int siv = s[s.Size - 1] * 100 + (s.Size > 1 ? s[s.Size - 2] * 10 : 0);
                int div = siv / giv;
                int xiv = s.Size - division.Length - 1; // offset, must be >= 0

                if (div > 9)
                {
                    //div /= 10; xiv++;
                }
                if (div > 0)
                {
                    ArithmeticArray sub = arithmeticMultiplication(g, div, xiv);

                    if (s < sub) break;

                    d = d + new ArithmeticArray(div, false, xiv);
                    s = s - sub;
                }
                else
                {
                    break;
                }
            }
            while(s >= g) // continue to check remainer
            {
                s = s - g;
                d++;
            }
            if (by > 0 && !a._isPositive || by < 0 && a.IsPositive)
            {
                d._isPositive = false;
            }
            return d;
        }

        private static ArithmeticArray arithmeticMultiplication(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return null;

            ArithmeticArray r = new ArithmeticArray(0);

            if (a.IsZero || b.IsZero)
            {
                return r;
            }
            if (a.HighIndex == 0 && a.HighIndexValue == 1) // a is 1 or a is -1
            {
                r = new ArithmeticArray(b);
                r._isPositive = a.IsPositive == b.IsPositive;
                return r;
            }
            if (b.HighIndex == 0 && b.HighIndexValue == 1) // b is 1 or b is -1
            {
                r = new ArithmeticArray(a);
                r._isPositive = a.IsPositive == b.IsPositive;
                return r;
            }

            ArithmeticArray s = new ArithmeticArray(0);
            ArithmeticArray v = new ArithmeticArray(a, false); // get base, ignore the sign

            for(int i = 0; i < b.Size; i++)
            {
                if (b[i] != 0)
                {
                    r = arithmeticMultiplication(v, b[i], i);
                    s = s + r;
                }
            }

            s._isPositive = a._isPositive == b._isPositive;

            return s;
        }
        private static ArithmeticArray arithmeticMultiplication(ArithmeticArray a, int by = 1, int x = 0)
        {
            if (a == null || a.Size == 0) return null;

            if (a.IsZero)
            {
                return new ArithmeticArray(0);
            }
            if (a.HighIndex == 0 && a.HighIndexValue == 1) // a is 1 or a is -1
            {
                ArithmeticArray r = new ArithmeticArray(by * x);
                r._isPositive = a.IsPositive == r.IsPositive;
                return r;
            }
            if (-1 <= by && by <= 1)
            {
                ArithmeticArray r = by == 0 || a.IsZero ? new ArithmeticArray(0) : new ArithmeticArray(a);
                for(int i = 0; i < x && by != 0; i++) r._array.Insert(0, 0);
                r._isPositive = by >= 0 && r._isPositive || by < 0 && !r._isPositive;
                return r;
            }
            if (-10 > by || by > 10)
            {
                ArithmeticArray b = new ArithmeticArray(by, true, x);
                return arithmeticMultiplication(a, b);
            }

            ArithmeticArray s = new ArithmeticArray(a, false);
            ArithmeticArray v = new ArithmeticArray(a, false); // ignoring sign

            for(int i = Math.Abs(by); i > 1; i--)
            {
                s = s + v;
            }
            for(int i = 0; i < x; i++)
            {
                s._array.Insert(0, 0);
            }

            s._isPositive = by >= 0 && a._isPositive || by < 0 && !a._isPositive;

            return s;
        }

        private static ArithmeticArray changeValueByList(ArithmeticArray a, List<int> by)
        {
            if (a == null || by == null || by.Count <= 0) return a;

            a._array.Clear();

            for(int i = 0; i < by.Count; i++)
            {
                a._array.Add(by[i]);
            }
            return a;
        }

        private static bool checkNull(ArithmeticArray a)
        {
            if (a == null || a.Size == 0) return true; return false;
        }

        private static int compare(ArithmeticArray a, ArithmeticArray b, bool compareValueOnly = false)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0)
            {
                string message = "Invalid argument: ";

                if (a == null || b == null)
                {
                    message += "One of arguments is null.";
                }
                else // zero sized array
                {
                    message += "The size of an ArithmeticaArray should never be zero.";
                }

                throw new ArgumentNullException(message);
            }

            if (compareValueOnly == false)
            {
                if (a.IsPositive && b.IsPositive == false) return 1;
                if (b.IsPositive && a.IsPositive == false) return -1;
            }

            int factor = a.IsPositive && b.IsPositive ? 1 : (compareValueOnly ? 1 : -1);
            int result = 0;

            if (a.Size != b.Size)
            {
                result = factor * ((a.Size > b.Size) ? 1 : -1);
            }
            else // same size
            {
                for(int i = a.Size - 1; i >= 0; i--)
                {
                    if (a[i] != b[i])
                    {
                        result = factor * (a[i] > b[i] ? 1 : -1);
                        break;
                    }
                }
            }
            return result;
        }
        private static int compare(ArithmeticArray a, int b, bool compareValueOnly = false)
        {
            if (a == null || a.Size == 0)
            {
                string message = "Invalid argument: ";

                if (a == null)
                {
                    message += "One of arguments is null.";
                }
                else // zero sized array
                {
                    message += "The size of an ArithmeticaArray should never be zero.";
                }

                throw new ArgumentNullException(message);
            }

            if (compareValueOnly == false)
            {
                if (a.IsPositive && b < 0) return 1;
                if (a.IsPositive == false && b >= 0) return -1;
            }

            int factor = a.IsPositive && b >= 0 ? 1 : (compareValueOnly ? 1 : -1);
            int result = 0;

            if (b == 0)
            {
                result = a.IsZero ? 0 : (a.IsPositive ? 1 : -1);
            }
            else if (a.Size == 1 && -10 < b && b < 10)
            {
                result = (a[0] > b ? 1 : (a[0] < b ? -1 : 0)) * factor;
            }
            else // converting to string for comparison
            {
                string bsv = Math.Abs(b).ToString(CultureInfo.InvariantCulture);

                if (a.Size != bsv.Length)
                {
                    result = factor * ((a.Size > bsv.Length) ? 1 : -1);
                }
                else // same size (a.Size == bsv.Length)
                {
                    for(int i = 0; i < a.Size; i++)
                    {
                        // note: string index order is different from ArithmeticArray index order
                        int aiv = a[a.Size - i - 1];
                        int biv = bsv[i] - '0';
                        if (aiv != biv)
                        {
                            result = factor * (aiv > biv ? 1 : -1);
                            break;
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region static methods (overloading operators)

        public static ArithmeticArray operator *(ArithmeticArray a, ArithmeticArray b)
        {
            return arithmeticMultiplication(a, b);
        }
        public static ArithmeticArray operator *(ArithmeticArray a, int b)
        {
            return arithmeticMultiplication(a, b);
        }
        public static ArithmeticArray operator /(ArithmeticArray a, ArithmeticArray b)
        {
            return arithmeticDivision(a, b);
        }
        public static ArithmeticArray operator /(ArithmeticArray a, int b)
        {
            return arithmeticDivision(a, b);
        }
        public static ArithmeticArray operator +(ArithmeticArray a, ArithmeticArray b)
        {
            return arithmeticAddition(a, b);
        }
        public static ArithmeticArray operator +(ArithmeticArray a, int b)
        {
            return arithmeticAddition(a, b);
        }
        public static ArithmeticArray operator -(ArithmeticArray a, ArithmeticArray b)
        {
            var righthand = new ArithmeticArray(b);
            righthand._isPositive = !righthand.IsPositive;
            return arithmeticAddition(a, righthand);
        }
        public static ArithmeticArray operator -(ArithmeticArray a, int b)
        {
            return arithmeticAddition(a, -b);
        }
        public static ArithmeticArray operator ++(ArithmeticArray a)
        {
            return arithmeticAddition(a, 1, 0, true);
        }
        public static ArithmeticArray operator --(ArithmeticArray a)
        {
            return arithmeticAddition(a, -1, 0, true);
        }

        /*// overloading == and !=
        public static bool operator !=(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;

            int result = compare(a, b);

            return result == 0;
        }
        public static bool operator ==(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;
            int result = compare(a, b);
            return result == 0;
        }
        //*/

        public static bool operator <=(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;
            int result = compare(a, b);
            return result <= 0;
        }
        public static bool operator >=(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;
            int result = compare(a, b);
            return result >= 0;
        }
        public static bool operator <=(ArithmeticArray a, int b)
        {
            if (a == null || a.Size == 0) return false;
            int result = compare(a, b);
            return result <= 0;
        }
        public static bool operator >=(ArithmeticArray a, int b)
        {
            if (a == null || a.Size == 0) return false;
            int result = compare(a, b);
            return result >= 0;
        }
        public static bool operator <(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;
            int result = compare(a, b);
            return result < 0;
        }
        public static bool operator >(ArithmeticArray a, ArithmeticArray b)
        {
            if (a == null || b == null || a.Size == 0 || b.Size == 0) return false;
            int result = compare(a, b);
            return result > 0;
        }
        public static bool operator <(ArithmeticArray a, int b)
        {
            if (a == null || a.Size == 0) return false;
            int result = compare(a, b);
            return result < 0;
        }
        public static bool operator >(ArithmeticArray a, int b)
        {
            if (a == null || a.Size == 0) return false;
            int result = compare(a, b);
            return result > 0;
        }

        #endregion

        public int this[int index]
        {
            get { return _array[index]; }
            set {
                if (0 <= index && index < _array.Count &&
                    0 <= value && value < 10)
                {
                    _array[index] = Math.Abs(value % 10);
                }
            }
        }

        public int CompareTo(ArithmeticArray other)
        {
            if (other == null || other.Size == 0) return 1;

            return compare(this, other);
        }
        public int CompareTo(int otherInteger)
        {
            return compare(this, otherInteger);
        }

        public override bool Equals(object other)
        {
            bool result = false;

            if (other is ArithmeticArray)
            {
                var obj = other as ArithmeticArray;

                try
                {
                    result = compare(this, obj) == 0;
                }
                catch (Exception)
                {
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            return this._array.GetHashCode();
        }

        public int[] ToArray(out bool isPositive)
        {
            int[] array = new int[this.Size];

            for(int i = 0; i < array.Length; i++)
            {
                array[i] = this._array[i];
            }

            isPositive = this.IsPositive;

            return array;
        }

        public override string ToString()
        {
            string arrayString = _isPositive ? String.Empty : "-";

            for(int i = _array.Count - 1; i >= 0; i--)
            {
                arrayString += _array[i].ToString(CultureInfo.InvariantCulture);
            }

            return arrayString;
        }

    }
}
