using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IFeedStreamRepository
    {
        IEnumerable<IStream> Load(byte[] feedId);

        void AttachStream(IStream feedStream);

        void DetachStream(IStream feedStream);
    }

    public class FeedStreamRepository : IFeedStreamRepository
    {
        readonly IFeedStreamStore store;

        public FeedStreamRepository(IFeedStreamStore store)
        {
            this.store = store;
        }

        public void AttachStream(IStream feedStream)
        {
            store.Save(feedStream);
        }

        public void DetachStream(IStream feedStream)
        {
            store.Delete(feedStream);
        }

        public IEnumerable<IStream> Load(byte[] feedId)
        {
            return store.Load(feedId);
        }
    }
}
