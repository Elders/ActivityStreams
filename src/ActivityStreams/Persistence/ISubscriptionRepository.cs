using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface ISubscriptionRepository
    {
        void Save(Subscription subsription);
        void Delete(Subscription subscription);
        Subscription Load(byte[] subscriptionId);
        IEnumerable<Subscription> LoadOwnerSubscriptions(byte[] ownerId);
    }
}