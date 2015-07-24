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
                var subscriptionId = Encoding.UTF8.GetBytes("subscriptionId");
                var ownerId = Encoding.UTF8.GetBytes("ownerId");
                subscription = new Subscription(subscriptionId, ownerId);
                var firstStreamId = Encoding.UTF8.GetBytes("streamId");
                subscription.SubscribeTo(firstStreamId);

                secondStreamId = Encoding.UTF8.GetBytes("streamId");
            };

        Because of = () => subscription.SubscribeTo(secondStreamId);

        It should_be_threated_as_a_single_subscribtion = () => subscription.Streams.Count().ShouldEqual(1);

        static Subscription subscription;
        static byte[] secondStreamId;
    }
}