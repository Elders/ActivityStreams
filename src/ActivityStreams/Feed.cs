using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivityStreams
{
    public class Feed
    {
        const int feedStreamLimit = 40000;
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
            if (feedStreams.Count > feedStreamLimit)
                throw new NotImplementedException("Cassandra supports up to 55k items in List. Do a new implementation hahaha.");

            feedStreams.Add(feedStream);
        }

        public void DetachStream(FeedStream feedStream)
        {
            feedStreams.Remove(feedStream);
        }
    }
}
