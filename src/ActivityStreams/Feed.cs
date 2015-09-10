using System.Collections.Generic;
using System.Linq;

namespace ActivityStreams
{
    public class Feed
    {
        readonly HashSet<FeedStream> feedStreams;

        public Feed(byte[] id)
        {
            Id = id;
            feedStreams = new HashSet<FeedStream>();
        }

        public byte[] Id { get; }

        public IEnumerable<byte[]> FeedStreams { get { return feedStreams.Select(x => x.StreamId).ToList(); } }

        public void AttachStream(FeedStream feedStream)
        {
            feedStreams.Add(feedStream);
        }

        public void DetachStream(FeedStream feedStream)
        {
            feedStreams.Remove(feedStream);
        }
    }
}
