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
                var ownerId = Encoding.UTF8.GetBytes("ownerId");
                feed = new ActivityFeed(ownerId);
                var firstStreamId = Encoding.UTF8.GetBytes("streamId");
                feed.AddStream(firstStreamId);

                secondStreamId = Encoding.UTF8.GetBytes("streamId");
            };

        Because of = () => feed.AddStream(secondStreamId);

        It should_be_threated_as_a_single_subscribtion = () => feed.Streams.Count().ShouldEqual(1);

        static ActivityFeed feed;
        static byte[] secondStreamId;
    }
}