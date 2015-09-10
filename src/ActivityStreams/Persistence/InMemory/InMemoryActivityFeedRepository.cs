using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryActivityFeedRepository : IActivityRepository
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
        public IEnumerable<Activity> Load(Feed feed)
        {
            var snapshot = new Dictionary<byte[], Queue<Activity>>(activityStreamStore.Count);
            foreach (var item in activityStreamStore)
            {
                snapshot.Add(item.Key, new Queue<Activity>(item.Value));
            }

            SortedSet<Activity> buffer = new SortedSet<Activity>(Activity.Comparer);
            var streams = feed.FeedStreams.ToList();
            var streamsCount = streams.Count;

            //  Init
            for (int streamIndexInsideSubsciption = 0; streamIndexInsideSubsciption < streamsCount; streamIndexInsideSubsciption++)
            {
                var streamId = streams[streamIndexInsideSubsciption];
                var activity = snapshot[streamId].Dequeue();
                buffer.Add(activity);
            }

            while (buffer.Count > 0)
            {
                Activity nextActivity = buffer.FirstOrDefault();
                buffer.Remove(nextActivity);
                var streamQueue = snapshot[nextActivity.StreamId];
                if (streamQueue.Count > 0)
                {
                    var candidate = snapshot[nextActivity.StreamId].Dequeue();
                    buffer.Add(candidate);
                }
                yield return nextActivity;
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
