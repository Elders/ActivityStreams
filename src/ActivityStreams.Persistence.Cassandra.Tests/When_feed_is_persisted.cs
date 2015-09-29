using System;
using System.Reflection;
using System.Text;
using Cassandra;
using Machine.Specifications;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;
using System.Linq;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_feed_is_persisted
    {
        Establish context = () =>
        {
            session = SessionCreator.Create();
            serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            var store = new FeedStreamStore(session);
            feedRepository = new FeedStreamRepository(store);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateFeedsStorage();

            var shit = DateTime.UtcNow;
            timestamp = shit.ToFileTimeUtc();
            feedId = Encoding.UTF8.GetBytes("feedId" + timestamp);
            var streamId = Encoding.UTF8.GetBytes("strid" + timestamp);
            feedFactory = new FeedFactory(feedRepository);
            feed = feedFactory.GG(feedId);
            feedStream = new Stream(feedId, streamId);
        };

        Because of = () => feed.Attach(feedStream);

        It should_have_the_attached_feed_stream = () =>
        {
            var feed = feedFactory.GG(feedId);
            feed.Streams.Count().ShouldEqual(1);
        };


        static byte[] feedId;
        static long timestamp;

        static ISerializer serializer;
        static ISession session;
        static IFeedStreamRepository feedRepository;
        static Feed feed;
        static FeedFactory feedFactory;
        static IStream feedStream;
    }


    [Subject("")]
    public class When_feed_has_one_stream
    {
        Establish context = () =>
        {
            session = SessionCreator.Create();
            serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            var store = new FeedStreamStore(session);
            feedRepository = new FeedStreamRepository(store);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateFeedsStorage();

            var shit = DateTime.UtcNow;
            timestamp = shit.ToFileTimeUtc();
            feedId = Encoding.UTF8.GetBytes("feedId" + timestamp);
            theStreamId = "strid" + timestamp;
            var streamId = Encoding.UTF8.GetBytes(theStreamId);
            feedFactory = new FeedFactory(feedRepository);
            feed = feedFactory.GG(feedId);
            feedStream = new Stream(feedId, streamId);
        };

        Because of = () => feed.Attach(feedStream);

        It should_have_the_attached_feed_stream = () =>
        {
            var feed = feedFactory.GG(feedId);
            var loadedStream = System.Text.Encoding.UTF8.GetString(feed.Streams.First().StreamId);
            theStreamId.ShouldEqual(loadedStream);
        };

        static string theStreamId;
        static byte[] feedId;
        static long timestamp;

        static ISerializer serializer;
        static ISession session;
        static IFeedStreamRepository feedRepository;
        static Feed feed;
        static FeedFactory feedFactory;
        static IStream feedStream;
    }
}
