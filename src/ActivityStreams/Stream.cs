using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams
{

    public class Stream : IStream, IEqualityComparer<Stream>, IEquatable<Stream>
    {
        public Stream(byte[] feedId, byte[] streamId)
        {
            FeedId = feedId;
            StreamId = streamId;
        }

        public byte[] FeedId { get; set; }

        public byte[] StreamId { get; set; }

        public IEnumerable<IStream> Streams { get { yield return this; } }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Stream);
        }

        public bool Equals(Stream other)
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

        public bool Equals(Stream left, Stream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public int GetHashCode(Stream obj)
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

        public void Attach(IStream feedStream)
        {
            throw new NotImplementedException();
        }

        public void Detach(IStream feedStream)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Stream left, Stream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Stream left, Stream right)
        {
            return !(left == right);
        }
    }
}
