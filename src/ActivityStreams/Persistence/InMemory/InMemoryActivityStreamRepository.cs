using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryActivityStreamRepository : IActivityStreamRepository
    {
        ConcurrentDictionary<byte[], HashSet<Activity>> activityStreamStore = new ConcurrentDictionary<byte[], HashSet<Activity>>();

        public void Append(Activity activity)
        {
            HashSet<Activity> stream;
            if (activityStreamStore.TryGetValue(activity.StreamId, out stream))
                stream.Add(activity);
            else
                activityStreamStore.TryAdd(activity.StreamId, new HashSet<Activity>() { activity });
        }

        public IEnumerable<Activity> Load(Subscription subscription)
        {
            foreach (var streamId in subscription.Streams)
            {
                HashSet<Activity> stream;
                if (activityStreamStore.TryGetValue(streamId, out stream))
                {
                    foreach (var activity in stream)
                    {
                        yield return activity;
                    }
                }
            }
        }
    }
}
