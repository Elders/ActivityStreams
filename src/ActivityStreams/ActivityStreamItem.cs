using System;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class ActivityStreamItem : IEquatable<ActivityStreamItem>
    {
        ActivityStreamItem() { }

        public ActivityStreamItem(byte[] id, object body, object place, object author)
        {
            Id = id;
            Body = body;
            Place = place;
            Author = author;
        }

        public byte[] Id { get; }

        public object Body { get; }

        public object Place { get; }

        public object Author { get; }

        public int GetHashCode(ActivityStreamItem obj)
        {
            return obj.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!typeof(ActivityStreamItem).IsAssignableFrom(obj.GetType())) return false;
            return Equals((ActivityStreamItem)obj);
        }

        public virtual bool Equals(ActivityStreamItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ByteArrayHelper.Compare(Id, other.Id);
        }

        public override int GetHashCode()
        {
            unchecked { return 223 ^ ByteArrayHelper.ComputeHash(Id); }
        }

        public static bool operator ==(ActivityStreamItem left, ActivityStreamItem right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(ActivityStreamItem left, ActivityStreamItem right)
        {
            return !(left == right);
        }
    }

}
