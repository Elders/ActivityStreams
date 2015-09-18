using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class Activity : IEquatable<Activity>
    {
        Activity() { }

        Activity(byte[] streamId, byte[] id, object body, object author, DateTime timestamp)
        {
            if (ReferenceEquals(null, id) || id.Length == 0) throw new ArgumentNullException(nameof(id));
            if (ReferenceEquals(null, streamId) || streamId.Length == 0) throw new ArgumentNullException(nameof(streamId));
            if (ReferenceEquals(null, body)) throw new ArgumentNullException(nameof(body));
            //if (ReferenceEquals(null, author)) throw new ArgumentNullException(nameof(author));

            ExternalId = id;
            StreamId = streamId;
            Body = body;
            Author = author;
            Timestamp = timestamp.ToFileTimeUtc();
        }

        public Activity(byte[] id, byte[] streamId, object body, object author)
            : this(streamId, id, body, author, DateTime.UtcNow)
        { }

        public byte[] ExternalId { get; }

        public byte[] StreamId { get; }

        public object Body { get; }

        public object Author { get; }

        public long Timestamp { get; }

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
            return ByteArrayHelper.Compare(ExternalId, other.ExternalId);
        }

        public override int GetHashCode()
        {
            unchecked { return 223 ^ ByteArrayHelper.ComputeHash(ExternalId); }
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

        static IComparer<Activity> comparerInstance = new ActivityComparer();
        public static IComparer<Activity> Comparer = comparerInstance;

        public static Activity UnitTestFactory(byte[] id, byte[] streamId, object body, object author, DateTime timestamp)
        {
            return new Activity(streamId, id, body, author, timestamp);
        }

        class ActivityComparer : IComparer<Activity>
        {
            public int Compare(Activity x, Activity y)
            {
                if (x == y) return 0;
                var compareResult = Comparer<long>.Default.Compare(x.Timestamp, y.Timestamp);
                if (compareResult == 0) return Comparer<int>.Default.Compare(x.GetHashCode(), y.GetHashCode());
                return compareResult;
            }
        }
    }

    public class ActivityMeta
    {
        public Dictionary<string, object> MetaCollection { get; set; }
    }
}
