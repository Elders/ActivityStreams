using System;
using System.Collections.Generic;
using ActivityStreams.Persistence.InMemory;

namespace ActivityStreams.Persistence.Cassandra
{
    public class ActivityRepository : IActivityRepository
    {
        ActivityStore store;

        public ActivityRepository(ActivityStore store)
        {
            this.store = store;
        }

        public void Append(Activity activity)
        {
            store.Save(activity);
        }

        public IEnumerable<Activity> Load(Feed feed)
        {
            return Load(feed, DateTime.UtcNow);
        }

        public IEnumerable<Activity> Load(Feed feed, DateTime timestamp)
        {
            var result = store.Get(feed, new Paging(timestamp.ToFileTimeUtc(), 20));
            return result;
        }
    }
}
