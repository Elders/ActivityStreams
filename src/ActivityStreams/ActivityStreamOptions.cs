using System;

namespace ActivityStreams
{
    public class ActivityStreamOptions
    {
        public ActivityStreamOptions(Paging paging, SortOrder sortOrder)
        {
            Paging = paging;
            SortOrder = sortOrder;
        }

        public Paging Paging { get; set; }
        public SortOrder SortOrder { get; set; }

        public static ActivityStreamOptions Default { get { return new ActivityStreamOptions(Paging.Default, SortOrder.Descending); } }
    }

    public enum SortOrder
    {
        Descending = -1,
        Unspecified = 0,
        Ascending = 1
    }

    public class Paging
    {
        public Paging(long timestamp, int take)
        {
            Timestamp = timestamp;
            Take = take;
        }

        public long Timestamp { get; private set; }

        public int Take { get; private set; }

        public static Paging Default { get { return new Paging(DateTime.UtcNow.ToFileTimeUtc(), 20); } }
    }
}
