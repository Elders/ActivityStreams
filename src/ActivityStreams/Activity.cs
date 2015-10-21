using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;
using System.Runtime.Serialization;

namespace ActivityStreams
{
    [DataContract(Name = "5896a266-2cb2-4814-ac5c-506fe4b4f50e")]
    public class Activity : IEquatable<Activity>
    {
        Activity() { }

        public Activity(byte[] streamId, byte[] id, object body, string author, DateTime timestamp)
        {
            if (ReferenceEquals(null, id) || id.Length == 0) throw new ArgumentNullException(nameof(id));
            if (ReferenceEquals(null, streamId) || streamId.Length == 0) throw new ArgumentNullException(nameof(streamId));
            if (ReferenceEquals(null, body)) throw new ArgumentNullException(nameof(body));
            if (ReferenceEquals(null, author)) throw new ArgumentNullException(nameof(author));

            ExternalId = id;
            StreamId = streamId;
            Body = body;
            Author = author;
            Timestamp = timestamp.ToFileTimeUtc();
        }

        public Activity(byte[] streamId, byte[] id, object body, string author)
            : this(streamId, id, body, author, DateTime.UtcNow)
        { }

        [DataMember(Order = 1)]
        public byte[] StreamId { get; private set; }

        [DataMember(Order = 2)]
        public long Timestamp { get; private set; }

        [DataMember(Order = 3)]
        public object Body { get; private set; }

        /// <summary>
        /// Reference back to an object inside system which generated the activity. Usually it is used to identify idempotency.
        /// </summary>
        [DataMember(Order = 4)]
        public byte[] ExternalId { get; private set; }

        [DataMember(Order = 5)]
        public string Author { get; private set; }

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

        static IComparer<Activity> comparerInstanceDesc = new ActivityComparerDesc();
        public static IComparer<Activity> ComparerDesc = comparerInstanceDesc;

        static IComparer<Activity> comparerInstanceAsc = new ActivityComparerAsc();
        public static IComparer<Activity> ComparerAsc = comparerInstanceAsc;

        class ActivityComparerDesc : IComparer<Activity>
        {
            public int Compare(Activity x, Activity y)
            {
                if (x == y) return 0;
                var compareResult = Comparer<long>.Default.Compare(x.Timestamp, y.Timestamp);
                if (compareResult == 0) return Comparer<int>.Default.Compare(x.GetHashCode(), y.GetHashCode());
                return compareResult;
            }
        }

        class ActivityComparerAsc : IComparer<Activity>
        {
            public int Compare(Activity x, Activity y)
            {
                if (x == y) return 0;
                var compareResult = Comparer<long>.Default.Compare(y.Timestamp, x.Timestamp);
                if (compareResult == 0) return Comparer<int>.Default.Compare(y.GetHashCode(), x.GetHashCode());
                return compareResult;
            }
        }
    }

    public class ActivityMeta
    {
        public Dictionary<string, object> MetaCollection { get; set; }
    }
}
