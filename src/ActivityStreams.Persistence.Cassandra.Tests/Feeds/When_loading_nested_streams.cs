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
            streamService.Attach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("str1-top_str_id" + sandbox));
            streamService.Attach(Encoding.UTF8.GetBytes("nested_level_1" + sandbox), Encoding.UTF8.GetBytes("str2-nested_level_1" + sandbox));
        };

        It should_return_only_top_level_streams = () => streamService.Get(Encoding.UTF8.GetBytes("top" + sandbox)).Streams.Count().ShouldEqual(1);
    }
}

