using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_a_feed_stream_is_added_twice_to_a_feed
    {
        Establish context = () =>
            {
                var feedId = Encoding.UTF8.GetBytes("ownerId");
                var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));
                feed = feedFactory.Get(feedId);
                var firstStreamId = Encoding.UTF8.GetBytes("streamId");
                feed.Attach(new Stream(feed.FeedId, firstStreamId));

                secondStreamId = Encoding.UTF8.GetBytes("streamId");
            };

        Because of = () => feed.Attach(new Stream(feed.FeedId, secondStreamId));

        It should_be_threated_as_a_single_subscribtion = () => feed.Streams.Count().ShouldEqual(1);

        static IFeed feed;
        static byte[] secondStreamId;
    }
}
