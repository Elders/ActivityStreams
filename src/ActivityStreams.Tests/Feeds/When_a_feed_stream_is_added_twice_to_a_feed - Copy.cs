using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_a_feed_stream_is_added_twice_to_a_feed_GG
    {
        Establish context = () =>
            {

                var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));
                var creator = new TestFeedCreator(feedFactory);

                var feed1 = creator.GetFeed("feed1");
                var feed2 = creator.GetFeed("feed2");

                feed1.Attach(feed2);

                feed = feed1;
                //var result = feed1.Streams;
            };

        //Because of = () => feed.Attach(new FeedStream(feed.Id, secondStreamId));

        It should_be_threated_as_a_single_subscribtion = () => feed.Streams.Count().ShouldEqual(4);

        static IStream feed;
    }

    public class TestFeedCreator
    {
        readonly FeedFactory feedFactory;

        public TestFeedCreator(FeedFactory feedFactory)
        {
            this.feedFactory = feedFactory;
        }

        public IStream GetFeed(string feedName)
        {
            var feed1Id = Encoding.UTF8.GetBytes(feedName);
            var feed = feedFactory.GG(feed1Id);
            for (int i = 0; i < 2; i++)
            {
                var feedStream = Encoding.UTF8.GetBytes($"{feedName}-stream-{i}");
                feed.Attach(new Stream(feed.Id, feedStream));
            }

            return feed;
        }
    }
}
