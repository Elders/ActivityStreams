using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Persistence;

namespace ActivityStreams
{
    public interface IFeedable
    {
        /// <summary>
        /// Feeds with more food.
        /// </summary>
        /// <param name="feedStream"></param>
        void AttachStream(FeedStream feedStream);
        void DetachStream(FeedStream feedStream);
    }

    public class Feed
    {
        readonly HashSet<FeedStream> feedStreams;
        readonly IFeedStreamRepository repository;

        internal Feed(byte[] id, IFeedStreamRepository repository)
        {
            this.Id = id;
            this.feedStreams = new HashSet<FeedStream>(repository.Load(id));
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
            return new Feed(feedId, repository);
        }
    }
}
