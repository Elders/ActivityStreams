using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryFeedStreamStore : IFeedStreamStore
    {
        ConcurrentDictionary<byte[], List<FeedStream>> activityFeedStore = new ConcurrentDictionary<byte[], List<FeedStream>>(new ByteArrayEqualityComparer());

        public void Delete(FeedStream feedStream)
        {
            List<FeedStream> feedStreams = new List<FeedStream>();
            if (activityFeedStore.TryGetValue(feedStream.FeedId, out feedStreams) == false)
                activityFeedStore.TryAdd(feedStream.FeedId, feedStreams);

            feedStreams.Remove(feedStream);
        }

        public IEnumerable<FeedStream> Load(byte[] feedId)
        {
            List<FeedStream> feedStreams;
            if (activityFeedStore.TryGetValue(feedId, out feedStreams))
                return feedStreams;

            return Enumerable.Empty<FeedStream>();
        }

        public void Save(FeedStream feedStream)
        {
            List<FeedStream> feedStreams;
            if (activityFeedStore.TryGetValue(feedStream.FeedId, out feedStreams) == false)
            {
                feedStreams = new List<FeedStream>();
                activityFeedStore.TryAdd(feedStream.FeedId, feedStreams);
            }

            feedStreams.Add(feedStream);
        }
    }
}
