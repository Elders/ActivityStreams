using System.Reflection;
using System.Text;
using Cassandra;
using Machine.Specifications;
using System;
using System.IO;
using ActivityStreams.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_feed_is_persisted
    {
        Establish context = () =>
        {
            session = SessionCreator.Create();
            serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            var store = new FeedStore(session, serializer);
            feedRepository = new FeedRepo(store);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateFeedsStorage();

            var shit = DateTime.UtcNow;
            timestamp = shit.ToFileTimeUtc();
            feedId = Encoding.UTF8.GetBytes("feedId" + timestamp);
            var streamId = Encoding.UTF8.GetBytes("strid" + timestamp);
            feed = new Feed(feedId);

            var feedStream = new FeedStream(feedId, streamId);

            feed.AttachStream(feedStream);
        };

        Because of = () => feedRepository.Save(feed);

        It should_have_the_activity_stored_properly = () =>
        {
            3456.ShouldBeOfExactType<int>();
        };


        static byte[] feedId;
        static long timestamp;

        static ISerializer serializer;
        static ISession session;
        static IFeedRepository feedRepository;
        static Feed feed;
    }
}
