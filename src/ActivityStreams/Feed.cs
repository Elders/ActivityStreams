using System;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Persistence;

namespace ActivityStreams
{
    public class Feed : IFeed
    {
        readonly HashSet<IStream> feedStreams;

        readonly IFeedStreamRepository repository;

        internal Feed(byte[] id, IFeedStreamRepository repository)
        {
            this.feedStreams = new HashSet<IStream>(repository.Load(id));
            this.FeedId = id;
            this.repository = repository;
        }

        public byte[] StreamId { get; set; }

        public byte[] FeedId { get; set; }

        public IEnumerable<IStream> Streams { get { return feedStreams.SelectMany(x => x.Streams).ToSet(); } }

        public void Attach(IStream feedStream)
        {
            if (feedStreams.Add(feedStream))
                repository.AttachStream(feedStream);
        }

        public void Detach(IStream feedStream)
        {
            if (feedStreams.Remove(feedStream))
                repository.DetachStream(feedStream);
        }

        public void Close(DateTime endDate)
        {

        }
    }

    public static class CollectionExtensions
    {
        public static ISet<TSource> ToSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }
    }

    public class FeedFactory
    {
        readonly IFeedStreamRepository repository;

        public FeedFactory(IFeedStreamRepository repository)
        {
            this.repository = repository;
        }

        public IFeed Get(byte[] feedId)
        {
            return new Feed(feedId, repository);
        }
    }
}
