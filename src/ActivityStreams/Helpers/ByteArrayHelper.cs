using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ActivityStreams.Helpers
{
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            return ((ReadOnlySpan<byte>)x).SequenceCompareTo(y) == 0;
        }

        public int GetHashCode(byte[] obj)
        {
            return ByteArrayHelper.ComputeHash(obj);
        }

        private static ByteArrayEqualityComparer instance = new ByteArrayEqualityComparer();

        public static ByteArrayEqualityComparer Default { get { return instance; } }
    }

    public static class ByteArrayHelper
    {
        public static bool Compare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return ((ReadOnlySpan<byte>)b1).SequenceCompareTo(b2) == 0;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {

            byte[] rv = new byte[first.Length + second.Length];
            System.Buffer.BlockCopy(first, 0, rv, 0, first.Length);
            System.Buffer.BlockCopy(second, 0, rv, first.Length, second.Length);
            return rv;
        }

        public static int ComputeHash(params byte[] data)
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < data.Length; i++)
                    hash = (hash ^ data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }
    }
}
