/*
 ************************************************************
 * Source: Common.cs
 * System: Microsoft Windows with .NET Framework
 * Author: Jason Zhu <jason_zhuyx@hotmail.com>
 * Update: 2011-01-21 Initial version
 *         
 ************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Common.Extensions;


namespace Common.Library
{
    public class Generic
    {
        public const double Pi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609;

        public static readonly string PiString = "3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609";

        public const int CHAR_BIT = 8;

        #region Algorithms

        /// See http://graphics.stanford.edu/~seander/bithacks.html
        public static int Abs(int v)
        {
            if (v == Int32.MinValue)
            {
                throw new OverflowException();
            }
            long mask = v >> sizeof(int) * CHAR_BIT - 1;
            long abs = (v + mask) ^ mask; // patented variation: abs = (v ^ mask) - mask;

            return (int)abs;
        }

        public static bool CompareAbs(double a, double b, double epsilon = 0.0000001)
        {
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(absA - absB);

            bool isSame = diff < epsilon;

            return isSame;
        }
        public static bool CompareAbs(float a, float b, float epsilon = 0.0000001f)
        {
            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(absA - absB);

            bool isSame = diff < epsilon;

            return isSame;
        }

        public static bool IsPowerOfTwo(int x)
        {
            return (x > 0) && ((x & (x - 1)) == 0);
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;

            for(int k = 2; k <= Math.Ceiling(Math.Sqrt(number)); k++)
            {
                if (number % k == 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Sieve Of Eratosthenes. See http://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPrimeBySieveOfEratosthenes(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;

            List<int> primes = 
                Enumerable.Range(0, number).Aggregate
                (
                    Enumerable.Range(2, Int32.MaxValue - 1).ToList(),
                    (result, index) =>
                    {
                        result.RemoveAll(i => i > result[index] && i % result[index] == 0);
                        return result;
                    }
                );

            return primes.Contains(number);
        }

        public static bool IsPrimeByLinq(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;

            /*//See http://www.codethinked.com/the-tekpub-linq-challenge-and-the-sieve-of-eratosthenes
            var maxInt = Int32.MaxValue;
            var rangeX = maxInt / 10000;
            var primes = Enumerable.Range(2, (int)Math.Sqrt(rangeX - 1) + 2).Aggregate
                (
                Enumerable.Range(2, rangeX).ToArray(), (sieve, i) =>
                    {
                        if (sieve[i - 2] == 0)
                        {
                            return sieve;
                        }
                        for(int m = 2; m <= rangeX / i; m++)
                        {
                            sieve[i * m - 2] = 0;
                        }
                        return sieve;
                    }
                ).Where(n => n != 0);
            //*/

            bool isAPrime = (Enumerable.Range(1, number).Count(x => number % x == 0) == 2);
            bool inPrimes = false; //primes.Contains(number);

            return isAPrime || inPrimes;
        }

        /// <summary>
        /// Newton-Raphson square root
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Sqrt(double x)
        {
            const double epsilon = 0.00000000000001d;

            if (x < 0) return -1d;

            double guess = x / 2.0d;
            double diff = (guess * guess) - x;
            double absDiff = (diff >= 0d ? diff : -diff);

            while (absDiff > epsilon)
            {
                guess = guess - (diff / (2.0d * guess));
                diff = (guess * guess) - x;
                absDiff = (diff >= 0d ? diff : -diff);
            }

            return guess;
        }

        #endregion

        #region DateTime Functions

        public static double GetAngleByTime(int hour, int minute, int second, bool ignoringSecond = false, bool angleDirection = false, bool clockDialBy24Hours = false)
        {
            double hrsFraction = clockDialBy24Hours ? 24 : 12;
            double angleSecond = 360 * second / 60.0;
          //double angleMinute = 360 *(minute / 60.0 + (ignoringSecond ? 0 : second / 3600.0);
            double angleMinute = 360 * minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);
          //double angleHour = ((360 * (hour % hrsFraction)) + angleMinute) / hrsFraction;
            double angleHour = ( 360 *((hour % hrsFraction) + minute / 60.0 + second / 3600.0)) / hrsFraction;
            double angle = angleHour - angleMinute;
            double angleAbs = Math.Abs((-180 > angle || angle > 180)? (360 - Math.Abs(angle)) : angle);

            return angleDirection ? angle : angleAbs;
        }
        public static double GetAngleOfHourHand(int hour, int minute, int second, bool ignoringSecond = false, bool clockDialBy24Hours = false)
        {
            double hrsFraction = clockDialBy24Hours ? 24 : 12;
            double angleSecond = 360 * second / 60.0;
            double angleMinute = 360 * minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);
            double angleHour = ((360 *(hour % hrsFraction)) + angleMinute) / hrsFraction;

            return angleHour;
        }
        public static double GetAngleOfMinuteHand(int hour, int minute, int second, bool ignoringSecond = false)
        {
            double angleSecond = 360 * second / 60.0;
            double angleMinute = 360 * minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);

            return angleMinute;
        }

        public static KeyValuePair<DateTime, double>[] GetTimeByAngle(double angle, out int countFound, bool angleDirection = false, bool by24Hours = false, int roundDigits = 2)
        {
            int maxHour = by24Hours ? 24 : 12;
            var current = DateTime.Now;
            var results = new List<KeyValuePair<DateTime, double>>();
            var epsilon = roundDigits > 0 ? 0.5 : 0.0;
            var moAngle = angle < -360.0 || angle > 360.0 ? angle.Mod(360.0) : angle;

            for(int i = 1; i < roundDigits; i++)
            {
                epsilon /= 10;
            }

            if (angleDirection == false)
            {
                moAngle = Math.Abs((moAngle < -180.0 || moAngle > 180.0) ? moAngle.Mod(180.0) : moAngle);
            }

            countFound = 0;

            //TODO: how to reduce O(h*m*s)
            double angleDiff = 360.0 / 60;
            int calcCounter = 0;

            for(int h = 0; h < maxHour; h++)
            {
               for(int m = 0; m < 60; m++)
               {
                    for(int s = 0; s < 60; s++)
                    {
                        var time = String.Format("{0:00}:{1:00}:{2:00}", h, m, s);

                        var angleDt = GetAngleByTime(h, m, s, false, angleDirection);

                        var diff = Math.Abs(angleDirection? (angleDt - moAngle) : Math.Abs(angleDt) - Math.Abs(moAngle));
                        var comp = CompareAbs(diff, 0.0, 0.001) || CompareAbs(angleDt, moAngle, 0.001);
                        var same = Math.Round(diff, roundDigits) < epsilon || comp;

                        if (same)
                        {
                            countFound++;
                            var dtFound = new DateTime(current.Year, current.Month, current.Day, h, m, s, DateTimeKind.Local);
                            var keyPair = new KeyValuePair<DateTime, double>(dtFound, angleDt);
                            results.Add(keyPair);
                        }
                        else if (diff > angleDiff)
                        {
                            int shift = (int)(diff / angleDiff);

                            if (shift > 0)
                            {
                                m += shift - 1; break;
                            }
                        }
                        calcCounter++;
                    }
                }
            }
            return results.ToArray();
        }

        #endregion

        #region Bits Reverser Functions
        /***********************************************************************
         * Reverse bit by bit in a buffer (i.e. exchange 1st bit and last bit of 
         * the buffer, exchange 2nd bit and the 2nd to last bit of the buffer). 
         * Assuming a buffer is a byte array, this function will reverse the
         * order of the items in array then reverse each byte.
         ***********************************************************************
         */
        public static byte[] ReverseBufferBits(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return buffer;

            byte[] bytes = new byte[buffer.Length];

            // reversing order of the items in a byte array
            //
            for(int i = 0; i < buffer.Length; i++)
            {
                bytes[i] = buffer[buffer.Length - i - 1];
            }
            /***************************************************
            *** Note: another way to swap in the same array
            ****************************************************
            /// byte[] bytes = (byte[])buffer.Clone();
            for(int i = 0; i <= bytes.Length / 2; i++)
            {
                byte temp = bytes[i];
                bytes[i] = bytes[bytes.Length - i - 1];
                bytes[bytes.Length - i - 1] = temp;
            }
            ****************************************************
            */

            // reversing each byte in new array
            //
            for(int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReverseBits(bytes[i]);
            }

            return bytes;
        }

        /***********************************************************************
         * Reverse bits functions
         * See http://graphics.stanford.edu/~seander/bithacks.html
         ***********************************************************************
         */

        public static byte ReverseBits(byte bits)
        {
            byte result;

            // swapping high bits and low bits
            int data = (bits >> 4) | ((bits & 0xf) << 4);

            // swapping between each 2-bit group
            data = ((data & 0xcc) >> 2) | ((data & 0x33) << 2);
            // swapping bits at the odd positions and even positions
            data = ((data & 0xaa) >> 1) | ((data & 0x55) << 1);

            result = (byte)data;

            return result;
        }

        public static byte ReverseBitsBy32bit(byte bits)
        {
            ulong data = ((bits * 0x0802LU & 0x22110LU) | (bits * 0x8020LU & 0x88440LU)) * 0x10101LU >> 16;

            return (byte)data;
        }
        public static byte ReverseBitsBy32bitProcessor(byte bits)
        {
            ulong data = (((bits * 0x80200802UL) & 0x0884422110UL) * 0x0101010101UL >> 34);

            return (byte)data;
        }

        public static byte ReverseBitsBy64bitProcessor(byte bits)
        {
            ulong data = (bits * 0x0202020202UL & 0x010884422010UL) % 1023;
            return (byte)data;
        }

        ///<summary>
        /// Reverse bits by looking up in a pre-calculated table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte ReverseBitsByLookup(byte bits)
        {
            byte[] reverseLookup = new byte[]
            {
                0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 
                0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
                0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 
                0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
                0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 
                0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
                0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC,
                0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
                0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2,
                0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
                0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA,
                0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA, 
                0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6,
                0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
                0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 
                0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE, 
                0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 
                0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
                0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 
                0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,  
                0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5,
                0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5, 
                0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED,
                0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD, 
                0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3,
                0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 
                0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 
                0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,  
                0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7,
                0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7, 
                0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF,
                0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
            };//32*8=256

            byte revb = reverseLookup[(int)bits];

            return revb;
        }

        public static byte ReverseBitsByLookup4x4(byte bits)
        {
            byte[] reverseLookup = new byte[]
            {
                0x00, 0x08, 0x04, 0x0C, 0x02, 0x0A, 0x06, 0x0E, 
                0x01, 0x09, 0x05, 0x0D, 0x03, 0x0B, 0x07, 0x0F
            };

            int hidx = (bits & 0xf0) >> 4;
            int ridx = (bits & 0x0f);

            byte revb = (byte)(
                (uint)reverseLookup[ridx] << 4 |
                (uint)reverseLookup[hidx]
                );

            return revb;
        }

        ///<summary>
        /// Bit Reverser by John Tobler (see http://weblogs.asp.net/jtobler/pages/156214.aspx)
        /// </summary>
        /// <param name="bits">A byte to be reversed</param>
        /// <returns>A new reversed byte</returns>
        public static byte ReverseBitsByMask(byte bits)
        {
            byte revb = 0x00;
            byte mask = 0x00;

            for (mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
            {
                revb >>= 1;
                byte temp = (byte)(bits & mask);

                if (temp != 0x00) revb |= 0x80;
            }

            return revb;
        }

        #endregion

    }// class Static


    public class Network
    {
        public static void Upload(string hostname, int port, string fileName)
        {
            TcpClient tcpClient = new TcpClient(hostname, port);
            NetworkStream networkStream = tcpClient.GetStream();
            FileStream fileStream = File.Open(fileName, FileMode.Open);// new FileStream(fileName, FileMode.Open, FileAccess.Read);

            int dataByte = fileStream.ReadByte();

            while(dataByte != -1)
            {
                networkStream.WriteByte((byte)dataByte);
                dataByte = fileStream.ReadByte();
            }

            fileStream.Close();
            networkStream.Close();
            tcpClient.Close();
        }
    }

    public enum Platform
    {
        X86,
        X64,
        Unknown
    }

    public class OS
    {
        internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        };

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public static Platform GetPlatform()
        {
            SYSTEM_INFO sysInfo = new SYSTEM_INFO();

            if (System.Environment.OSVersion.Version.Major > 5 || (
                System.Environment.OSVersion.Version.Major == 5 && 
                System.Environment.OSVersion.Version.Minor >= 1))
            {
                GetNativeSystemInfo(ref sysInfo);
            }
            else
            {
                GetSystemInfo(ref sysInfo);
            }

            switch (sysInfo.wProcessorArchitecture)
            {
                case PROCESSOR_ARCHITECTURE_IA64:
                case PROCESSOR_ARCHITECTURE_AMD64:
                    return Platform.X64;

                case PROCESSOR_ARCHITECTURE_INTEL:
                    return Platform.X86;

                default:
                    return Platform.Unknown;
            }
        }

        public static bool Is64Bit
        {
            get
            {
                Platform platform = GetPlatform();
                return platform == Platform.X64;
            }
        }

    }// class System


    /// <summary>
    /// Note:
    /// rand field has the [ThreadStatic] attribute applied to it, 
    /// so it's effectively a different variable for each thread.
    /// seedCounter is initialized (once) based on the current time, 
    /// but then increment it in a thread-safe manner each time 
    /// need a new instance for another thread.
    /// </summary>
    public static class RandomHelper
    {
        private static int _seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random rand;

        public static Random Instance
        {
            get
            {
                if (rand == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    rand = new Random(seed);
                }
                return rand;
            }
        }

        public static double NextDouble(double min = 0.0, double max = 1.0)
        {
            double result = min + Instance.NextDouble() * (max - min);
            return result;
        }

        public static int NextInteger(int min = 0, int max = 1)
        {
            int result = min + Instance.Next() * (max - min);
            return result;
        }
    }

}// namespace Common.Library
