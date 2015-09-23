namespace ActivityStreams.Persistence
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
    }
}
