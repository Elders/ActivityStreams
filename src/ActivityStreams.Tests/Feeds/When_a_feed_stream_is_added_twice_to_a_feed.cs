using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_a_feed_stream_is_added_twice_to_a_feed
    {
        Establish context = () =>
            {
                var feedId = Encoding.UTF8.GetBytes("ownerId");
                feed = new Feed(feedId);
                var firstStreamId = Encoding.UTF8.GetBytes("streamId");
                feed.AttachStream(new FeedStream(feed.Id, firstStreamId));

                secondStreamId = Encoding.UTF8.GetBytes("streamId");
            };

        Because of = () => feed.AttachStream(new FeedStream(feed.Id, secondStreamId));

        It should_be_threated_as_a_single_subscribtion = () => feed.FeedStreams.Count().ShouldEqual(1);

        static Feed feed;
        static byte[] secondStreamId;
    }
}
