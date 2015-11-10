using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_loading_nested_streams : CassandraContext
    {
        Establish context = () =>
        {
            streamService.Attach(Encoding.UTF8.GetBytes("top" + CassandraContext), Encoding.UTF8.GetBytes("str1-top_str_id" + CassandraContext));
            streamService.Attach(Encoding.UTF8.GetBytes("nested_level_1"), Encoding.UTF8.GetBytes("str2-nested_level_1"));
        };

        It should_return_only_top_level_streams = () => streamService.Get(Encoding.UTF8.GetBytes("top")).Streams.Count().ShouldEqual(1);
    }
}

