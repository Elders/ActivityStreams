using System.Collections.Concurrent;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryFeedRepository : IFeedRepository
    {
        ConcurrentDictionary<byte[], Feed> activityFeedStore = new ConcurrentDictionary<byte[], Feed>(new ByteArrayEqualityComparer());

        public Feed Get(byte[] id)
        {
            Feed feed;
            if (activityFeedStore.TryGetValue(id, out feed))
                return feed;

            return null;
        }

        public void Save(Feed feed)
        {
            activityFeedStore.TryAdd(feed.Id, feed);
        }
    }
}
