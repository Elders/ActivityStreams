using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryActivityStore : IActivityStore
    {
        ConcurrentDictionary<byte[], SortedSet<Activity>> activityStore =
            new ConcurrentDictionary<byte[], SortedSet<Activity>>(new ByteArrayEqualityComparer());

        public void Save(Activity activity)
        {
            SortedSet<Activity> stream;
            if (activityStore.TryGetValue(activity.StreamId, out stream))
                stream.Add(activity);
            else
                activityStore.TryAdd(activity.StreamId, new SortedSet<Activity>(Activity.ComparerDesc) { activity });
        }

        public IEnumerable<Activity> LoadStream(byte[] streamId, Paging paging)
        {
            SortedSet<Activity> stream;
            if (activityStore.TryGetValue(streamId, out stream))
            {
                var result = stream.Where(x => x.Timestamp <= paging.Timestamp).Take(paging.Take);
                foreach (var activity in result)
                {
                    yield return activity;
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
