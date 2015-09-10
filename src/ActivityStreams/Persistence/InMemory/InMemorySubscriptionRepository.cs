using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemorySubscriptionRepository : ISubscriptionRepository
    {
        ConcurrentDictionary<byte[], ActivityFeed> activityFeedStore = new ConcurrentDictionary<byte[], ActivityFeed>();

        public void Delete(Subscription subscription)
        {
            ActivityFeed feed;

            if (activityFeedStore.TryGetValue(subscription.OwnerId, out feed))
                feed.RemoveStream(subscription);
        }

        public ActivityFeed Load(byte[] ownerId)
        {
            ActivityFeed feed;

            if (activityFeedStore.TryGetValue(ownerId, out feed))
                return feed;

            return null;
        }

        public void Save(Subscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}
