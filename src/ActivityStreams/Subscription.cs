using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class Subscription : IEqualityComparer<Subscription>, IEquatable<Subscription>
    {
        public Subscription(byte[] ownerId, byte[] streamId)
        {
            OwnerId = ownerId;
            StreamId = streamId;
        }

        public byte[] OwnerId { get; set; }

        public byte[] StreamId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Subscription);
        }

        public bool Equals(Subscription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var t = GetType();
            if (t != other.GetType())
                return false;

            return
                 ByteArrayHelper.Compare(OwnerId, other.OwnerId) &&
                 ByteArrayHelper.Compare(StreamId, other.StreamId);
        }

        public bool Equals(Subscription left, Subscription right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public int GetHashCode(Subscription obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 463;
                int multiplier = 11;

                var ownerHash = ByteArrayHelper.ComputeHash(OwnerId);
                var streamIdHash = ByteArrayHelper.ComputeHash(StreamId);

                hashCode = hashCode * multiplier ^ ownerHash;
                hashCode = hashCode * multiplier ^ streamIdHash;

                return hashCode;
            }
        }

        public static bool operator ==(Subscription left, Subscription right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Subscription left, Subscription right)
        {
            return !(left == right);
        }
    }
}
