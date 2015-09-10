namespace ActivityStreams.Persistence
{
    public interface ISubscriptionRepository
    {
        void Save(Subscription subscription);
        void Delete(Subscription subscription);
        ActivityFeed Load(byte[] ownerId);
    }
}
