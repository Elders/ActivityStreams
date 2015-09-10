using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface ISubscriptionRepository
    {
        void Save(ActivityFeed subsription);
        void Delete(ActivityFeed subscription);
        ActivityFeed Load(byte[] subscriptionId);
        IEnumerable<ActivityFeed> LoadOwnerSubscriptions(byte[] ownerId);
    }
}