using System;
using System.Collections.Generic;

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
            return ComputeHash(obj);
        }

        private static ByteArrayEqualityComparer instance = new ByteArrayEqualityComparer();

        public static ByteArrayEqualityComparer Default { get { return instance; } }

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
