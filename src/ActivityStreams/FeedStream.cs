using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class FeedStream : IEqualityComparer<FeedStream>, IEquatable<FeedStream>
    {
        public FeedStream(byte[] feedId, byte[] streamId)
        {
            FeedId = feedId;
            StreamId = streamId;
        }

        public byte[] FeedId { get; set; }

        public byte[] StreamId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as FeedStream);
        }

        public bool Equals(FeedStream other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var t = GetType();
            if (t != other.GetType())
                return false;

            return
                 ByteArrayHelper.Compare(FeedId, other.FeedId) &&
                 ByteArrayHelper.Compare(StreamId, other.StreamId);
        }

        public bool Equals(FeedStream left, FeedStream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public int GetHashCode(FeedStream obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 463;
                int multiplier = 11;

                var ownerHash = ByteArrayHelper.ComputeHash(FeedId);
                var streamIdHash = ByteArrayHelper.ComputeHash(StreamId);

                hashCode = hashCode * multiplier ^ ownerHash;
                hashCode = hashCode * multiplier ^ streamIdHash;

                return hashCode;
            }
        }

        public static bool operator ==(FeedStream left, FeedStream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(FeedStream left, FeedStream right)
        {
            return !(left == right);
        }
    }
}
