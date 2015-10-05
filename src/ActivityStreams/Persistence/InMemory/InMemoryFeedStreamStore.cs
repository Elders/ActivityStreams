using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryFeedStreamStore : IFeedStreamStore
    {
        ConcurrentDictionary<byte[], List<IStream>> activityFeedStore = new ConcurrentDictionary<byte[], List<IStream>>(new ByteArrayEqualityComparer());

        public void Delete(IStream feedStream)
        {
            List<IStream> feedStreams = new List<IStream>();
            if (activityFeedStore.TryGetValue(feedStream.FeedId, out feedStreams) == false)
                activityFeedStore.TryAdd(feedStream.FeedId, feedStreams);

            feedStreams.Remove(feedStream);
        }

        public IEnumerable<IStream> Load(byte[] feedId)
        {
            List<IStream> feedStreams;
            if (activityFeedStore.TryGetValue(feedId, out feedStreams))
                return feedStreams;

            return Enumerable.Empty<IStream>();
        }

        public void Save(IStream feedStream)
        {
            List<IStream> feedStreams;
            if (activityFeedStore.TryGetValue(feedStream.FeedId, out feedStreams) == false)
            {
                feedStreams = new List<IStream>();
                activityFeedStore.TryAdd(feedStream.FeedId, feedStreams);
            }

            feedStreams.Add(feedStream);
        }
    }
}
