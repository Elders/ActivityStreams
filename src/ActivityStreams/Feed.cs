using System;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Persistence;

namespace ActivityStreams
{
    public interface IFeedable
    {

    }

    public class Feed
    {
        readonly HashSet<FeedStream> feedStreams;
        readonly IFeedStreamRepository repository;

        internal Feed(byte[] id, IEnumerable<FeedStream> feedStreams, IFeedStreamRepository repository)
        {
            Id = id;
            this.feedStreams = new HashSet<FeedStream>(feedStreams);
            this.repository = repository;
        }

        public byte[] Id { get; }

        public IEnumerable<byte[]> Streams { get { return feedStreams.Select(x => x.StreamId).ToList(); } }

        public void AttachStream(FeedStream feedStream)
        {
            if (feedStreams.Add(feedStream))
                repository.AttachStream(feedStream);
        }

        public void DetachStream(FeedStream feedStream)
        {
            if (feedStreams.Remove(feedStream))
                repository.DetachStream(feedStream);
        }
    }

    public class FeedFactory
    {
        readonly IFeedStreamRepository repository;

        public FeedFactory(IFeedStreamRepository repository)
        {
            this.repository = repository;
        }

        public Feed GG(byte[] feedId)
        {
            var feedStreams = repository.Load(feedId);
            return new Feed(feedId, feedStreams, repository);
        }
    }
}
