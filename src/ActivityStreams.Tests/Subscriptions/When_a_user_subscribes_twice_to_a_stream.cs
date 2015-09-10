using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Subscriptions
{
    [Subject("Subscriptions")]
    public class When_a_user_subscribes_twice_to_a_stream
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
