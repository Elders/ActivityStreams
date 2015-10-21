using System;

namespace ActivityStreams
{
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
