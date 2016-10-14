using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_a_stream_is_attached_twice : InMemoryContext
    {
        Establish context = () =>
        {
            topId = Encoding.UTF8.GetBytes("top" + sandbox);
            duplicatedId = Encoding.UTF8.GetBytes("duplicate" + sandbox);

            streamService.Attach(topId, duplicatedId);
            streamService.Attach(topId, duplicatedId);
        };

        Because of = () => streamService.Attach(topId, duplicatedId);

        It should_be_attached_only_once = () => streamService.Get(topId).Streams.Count().ShouldEqual(1);

        static byte[] topId;
        static byte[] duplicatedId;
    }
}
