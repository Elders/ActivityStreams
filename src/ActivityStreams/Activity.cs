using System;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class Activity : IEquatable<Activity>
    {
        Activity() { }

        public Activity(byte[] id, byte[] streamId, object body, object author)
        {
            if (ReferenceEquals(null, id) || id.Length == 0) throw new ArgumentNullException(nameof(id));
            if (ReferenceEquals(null, streamId) || streamId.Length == 0) throw new ArgumentNullException(nameof(streamId));
            if (ReferenceEquals(null, body)) throw new ArgumentNullException(nameof(body));
            if (ReferenceEquals(null, author)) throw new ArgumentNullException(nameof(author));

            Id = id;
            StreamId = streamId;
            Body = body;
            Author = author;
            Timestamp = DateTime.UtcNow;
        }

        public byte[] Id { get; }

        public byte[] StreamId { get; }

        public object Body { get; }

        public object Author { get; }

        public DateTime Timestamp { get; }

        public int GetHashCode(Activity obj)
        {
            return obj.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!typeof(Activity).IsAssignableFrom(obj.GetType())) return false;
            return Equals((Activity)obj);
        }

        public virtual bool Equals(Activity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ByteArrayHelper.Compare(Id, other.Id);
        }

        public override int GetHashCode()
        {
            unchecked { return 223 ^ ByteArrayHelper.ComputeHash(Id); }
        }

        public static bool operator ==(Activity left, Activity right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Activity left, Activity right)
        {
            return !(left == right);
        }
    }
}
