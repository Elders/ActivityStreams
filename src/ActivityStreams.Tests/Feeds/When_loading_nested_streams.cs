using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_loading_nested_streams : InMemoryContext
    {
        Establish context = () =>
        {
            streamService.Attach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("str1-top_str_id"));
            streamService.Attach(Encoding.UTF8.GetBytes("nested_level_1"), Encoding.UTF8.GetBytes("str2-nested_level_1"));
        };

        It should_return_only_top_level_streams = () => streamService.Get(Encoding.UTF8.GetBytes("top")).Streams.Count().ShouldEqual(1);
    }
}

