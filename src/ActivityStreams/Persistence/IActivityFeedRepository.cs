using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IFeedStreamRepository
    {
        IEnumerable<FeedStream> Load(byte[] feedId);

        void AttachStream(FeedStream feedStream);

        void DetachStream(FeedStream feedStream);
    }

    public class FeedStreamRepository : IFeedStreamRepository
    {
        readonly IFeedStreamStore store;

        public FeedStreamRepository(IFeedStreamStore store)
        {
            this.store = store;
        }

        public void AttachStream(FeedStream feedStream)
        {
            store.Save(feedStream);
        }

        public void DetachStream(FeedStream feedStream)
        {
            store.Delete(feedStream);
        }

        public IEnumerable<FeedStream> Load(byte[] feedId)
        {
            return store.Load(feedId);
        }
    }
}
