using System;
using System.Collections.Concurrent;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemorySubscriptionRepository : IActivityFeedRepository
    {
        ConcurrentDictionary<byte[], Feed> activityFeedStore = new ConcurrentDictionary<byte[], Feed>();

        public Feed Get(byte[] id)
        {
            Feed feed;
            if (activityFeedStore.TryGetValue(id, out feed))
                return feed;

            return null;
        }

        public Feed Save(Feed feed)
        {
            throw new NotImplementedException();
        }
    }
}
