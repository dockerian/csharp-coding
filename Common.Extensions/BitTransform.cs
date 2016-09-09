using System;
using System.Linq;

namespace Common.Extensions
{    
    /// <summary>
    /// Methods that assist in manipulating raw bytes and byte arrays.
    /// </summary>
    public static class BitTransform
    {
        public static byte[] CombineBytes(byte[] a1, byte[] a2)
        {
            byte[] result = new byte[a1.Length + a2.Length];
            Buffer.BlockCopy(a1, 0, result, 0, a1.Length);
            Buffer.BlockCopy(a2, 0, result, a1.Length, a2.Length);            
            return result;
        } 

        public static byte[] CombineBytes(byte[] a1, byte[] a2, byte[] a3)
        {
            byte[] result = new byte[a1.Length + a2.Length + a3.Length];
            Buffer.BlockCopy(a1, 0, result, 0, a1.Length);
            Buffer.BlockCopy(a2, 0, result, a1.Length, a2.Length);
            Buffer.BlockCopy(a3, 0, result, a1.Length + a2.Length, a3.Length);
            return result;
        }

        public static byte[] CombineBytes(byte[] a1, byte[] a2, byte[] a3, byte[] a4)
        {
            byte[] result = new byte[a1.Length + a2.Length + a3.Length];
            Buffer.BlockCopy(a1, 0, result, 0, a1.Length);
            Buffer.BlockCopy(a2, 0, result, a1.Length, a2.Length);
            Buffer.BlockCopy(a3, 0, result, a1.Length + a2.Length, a3.Length);
            Buffer.BlockCopy(a4, 0, result, a1.Length + a2.Length + a3.Length, a4.Length);
            return result;
        } 

        public static byte[] CombineBytes(params byte[][] arrays)
        {
            byte[] result = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
        }

        public static short SwapBytes(short number)
        {
            return (short)(((number >> 8) & 0xFF) | ((number << 8) & 0xFF00));
        }

        public static int SwapBytes(int number)
        {
            return (((number >> 24) & 0xFF)
                | ((number >> 08) & 0xFF00)
                | ((number << 08) & 0xFF0000)
                | ((number << 24)));
        }

        public static long SwapBytes(long number)
        {
            return (((number >> 56) & 0xFF)
                | ((number >> 40) & 0xFF00)
                | ((number >> 24) & 0xFF0000)
                | ((number >> 08) & 0xFF000000)
                | ((number << 08) & 0xFF00000000)
                | ((number << 24) & 0xFF0000000000)
                | ((number << 40) & 0xFF000000000000)
                | ((number << 56)));
        }

        public static short HostToNetworkOrder(short host)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (host);
            }

            return SwapBytes(host);
        }

        public static int HostToNetworkOrder(int host)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (host);
            }

            return SwapBytes(host);
        }

        public static long HostToNetworkOrder(long host)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (host);
            }

            return SwapBytes(host);
        }

        public static short NetworkToHostOrder(short network)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (network);
            }

            return SwapBytes(network);
        }

        public static int NetworkToHostOrder(int network)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (network);
            }

            return SwapBytes(network);
        }

        public static long NetworkToHostOrder(long network)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return (network);
            }

            return SwapBytes(network);
        }
        
    }    
}
