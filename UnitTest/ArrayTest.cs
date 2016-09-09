using Common.Extensions;
using Common.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;


namespace UnitTest
{
    [TestClass]
    public class ArrayTest
    {
        [TestMethod]
        public void ArrayConstructorTest()
        {
            ArithmeticArray a = new ArithmeticArray(55555);
            ArithmeticArray b = new ArithmeticArray(66666);

            Assert.AreEqual(a.ToString(), "55555");
            Assert.AreEqual(b.ToString(), "66666");
            Assert.AreEqual(a.CompareTo(b), -1);
            Assert.AreEqual(b.CompareTo(a), 1);
            Assert.AreEqual(b.CompareTo(99999), -1);
            Assert.AreEqual(a.CompareTo(0), 1);

            ArithmeticArray d = a - b;
            Assert.AreEqual(d.ToString(), "-11111");

            ArithmeticArray s = a + b;
            Assert.AreEqual(s.ToString(), "122221");
            Assert.AreEqual(s.CompareTo(0), 1);

            ArithmeticArray e = new ArithmeticArray("-7777777");
            s = a + e;
            Assert.AreEqual(s.ToString(), "-7722222");
            Assert.AreEqual(s.CompareTo(-7777777), 1);
            Assert.AreEqual(s.CompareTo(0), -1);

            bool sign;
            int[] array = s.ToArray(out sign);
            ArithmeticArray t = new ArithmeticArray(sign, array);
            Assert.IsTrue(s.CompareTo(t) == 0);
            Assert.IsTrue(s.Equals(t));

        }

        [TestMethod]
        public void ArrayOperatorTest()
        {
            ArithmeticArray a = new ArithmeticArray(0);
            Assert.AreEqual(a.ToString(), "0"); // a == 0

            a++;
            Assert.AreEqual(a.ToString(), "1"); // a == 1
            Assert.AreNotEqual(a.Size, 0);

            --a;
            Assert.AreEqual(a.ToString(), "0"); // a == 0
            Assert.AreNotEqual(a.Size, 0);
            --a;
            Assert.AreEqual(a.ToString(), "-1"); // a == -1
            Assert.AreNotEqual(a.Size, 0);
            --a;
            Assert.AreEqual(a.ToString(), "-2"); // a == -2
            Assert.AreNotEqual(a.Size, 0);

            a++;
            a++;
            Assert.AreEqual(a.ToString(), "0"); // a == 0
            Assert.AreNotEqual(a.Size, 0);

            int times = 10;
            for(int n = 0; n < times; n++)
            {
                --a;
            }
            Assert.AreEqual(a.ToString(), "-" + times.ToString()); // a == -10
            Assert.AreNotEqual(a.Size, 0);

            ArithmeticArray b = new ArithmeticArray(100000);
            Assert.AreEqual(b.ToString(), "100000");
            Assert.AreEqual(a.CompareTo(null), 1); // a > null
            Assert.AreEqual(a.CompareTo(-10), 0); // a == -10
            Assert.AreEqual(a.CompareTo(10), -1); // a < 10
            Assert.AreEqual(a.CompareTo(0), -1); // a < 0
            Assert.AreEqual(a.CompareTo(b), -1); // a < b
            Assert.AreEqual(b.CompareTo(-100000), 1); // b > -100000
            Assert.AreEqual(b.CompareTo(100001), -1); // b < 100001
            Assert.AreEqual(b.CompareTo(100000), 0); // b == 100000
            Assert.AreEqual(b.CompareTo(b), 0); // b == b
            Assert.AreEqual(b.CompareTo(a), 1); // b > a
            Assert.IsFalse(b <= a);
            Assert.IsFalse(b < a);
            Assert.IsTrue(b >= a);
            Assert.IsTrue(b > a);
            Assert.IsTrue(b <= 100000);
            Assert.IsTrue(b < 100001);
            Assert.IsTrue(b > 99999);
            Assert.IsTrue(b > -100000);
            Assert.IsTrue(b > 0);

            ArithmeticArray s = b * a;
            Assert.AreEqual(s.ToString(), "-1000000");

            s = b / a;
            Assert.AreEqual(s.ToString(), "-10000");
            s = b / -10;
            Assert.AreEqual(s.ToString(), "-10000");
            s = b / -999;
            Assert.AreEqual(s.ToString(), "-100");
            s = b / -9999;
            Assert.AreEqual(s.ToString(), "-10");
            s = b / 99999;
            Assert.AreEqual(s.ToString(), "1");
            s = b / 999999;
            Assert.AreEqual(s.ToString(), "0");

            a = new ArithmeticArray("32549853167");
            b = new ArithmeticArray("-97682");
            s = a / b;
            Assert.AreEqual(s.ToString(), "-333222");
            s = b / a;
            Assert.AreEqual(s.ToString(), "0");

            a = new ArithmeticArray("592347883169");
            b = new ArithmeticArray("-537682");
            s = a / b;
            Assert.AreEqual(s.ToString(), "-1101669");

            bool sign;
            int[] array = s.ToArray(out sign);
            array.IncreaseBy(10);
            ArithmeticArray t = new ArithmeticArray(sign, array);
            Assert.IsTrue(s.CompareTo(t) > 0);

            //Using BigInteger to verify
            string bigInteger = Generic.PiString.Replace(".", "");
            ArithmeticArray x = new ArithmeticArray(bigInteger);
            BigInteger bigInt = BigInteger.Parse(bigInteger);
            Assert.AreEqual(bigInt.ToString(), x.ToString());

            BigInteger oprand = BigInteger.Parse(s.ToString());
            BigInteger result = BigInteger.Multiply(bigInt, oprand);
            ArithmeticArray r = x * s;
            Assert.AreEqual(r.ToString(), result.ToString());
            result = BigInteger.Divide(bigInt, oprand);
            r = x / s;
            Assert.AreEqual(r.ToString(), result.ToString());

            int ai = 4832954;
            oprand = new BigInteger(ai);
            result = BigInteger.Add(bigInt, oprand);
            r = x + ai;
            Assert.AreEqual(r.ToString(), result.ToString());
            result = BigInteger.Subtract(bigInt, oprand);
            r = x - ai;
            Assert.AreEqual(r.ToString(), result.ToString());
            result = BigInteger.Multiply(bigInt, oprand);
            r = x * ai;
            Assert.AreEqual(r.ToString(), result.ToString());
            result = BigInteger.Divide(bigInt, oprand);
            r = x / ai;
            Assert.AreEqual(r.ToString(), result.ToString());

        }
    }
}
