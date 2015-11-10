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
            streamService.Attach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("duplicate"));
            streamService.Attach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("duplicate"));
        };

        Because of = () => streamService.Attach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("duplicate"));

        It should_be_attached_only_once = () => streamService.Get(Encoding.UTF8.GetBytes("top")).Streams.Count().ShouldEqual(1);
    }
}
