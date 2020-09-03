using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence.InMemory
{
    public class InMemoryStreamStore : IStreamStore
    {
        ConcurrentDictionary<byte[], ActivityStream> activityFeedStore =
            new ConcurrentDictionary<byte[], ActivityStream>(new ByteArrayEqualityComparer());

        public void Attach(byte[] sourceStreamId, byte[] streamIdToAttach, long expiresAt)
        {
            ActivityStream stream;
            if (activityFeedStore.TryGetValue(sourceStreamId, out stream) == false)
            {
                stream = new ActivityStream(sourceStreamId);
                activityFeedStore.TryAdd(sourceStreamId, stream);
            }
            var streamToAttach = new ActivityStream(streamIdToAttach, expiresAt);
            stream.AttachedStreams.Add(streamToAttach);
        }

        public void Detach(byte[] sourceStreamId, byte[] streamIdToDetach, long detachedSince)
        {
            ActivityStream stream;
            if (activityFeedStore.TryGetValue(sourceStreamId, out stream))
            {
                var detaching = stream.AttachedStreams.Where(x => ByteArrayHelper.Compare(x.StreamId, streamIdToDetach)).Single();
                detaching.ExpiresAt = detachedSince;
            }
        }

        public ActivityStream Get(byte[] streamId)
        {
            ActivityStream stream = null;
            activityFeedStore.TryGetValue(streamId, out stream);
            return stream;
        }

        public Task<ActivityStream> GetAsync(byte[] streamId)
        {
            return Task.FromResult(Get(streamId));
        }
    }
}
