using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryActivityStreamRepository : IActivityStreamRepository
    {
        ConcurrentDictionary<byte[], SortedSet<Activity>> activityStreamStore = new ConcurrentDictionary<byte[], SortedSet<Activity>>();

        public void Append(Activity activity)
        {
            SortedSet<Activity> stream;
            if (activityStreamStore.TryGetValue(activity.StreamId, out stream))
                stream.Add(activity);
            else
                activityStreamStore.TryAdd(activity.StreamId, new SortedSet<Activity>(Activity.Comparer) { activity });
        }

        /// <summary>
        /// FanIn
        /// </summary>
        public IEnumerable<Activity> Load(Subscription subscription)
        {
            foreach (var streamId in subscription.Streams)
            {
                SortedSet<Activity> stream;
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

    public class Paging
    {
        public Paging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; } = 5;

        public int Take { get; }

        public Paging Next(Paging paging)
        {
            return new Paging(paging.Skip + paging.Take, paging.Take);
        }
    }
}
