using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_a_stream_is_attached_twice : CassandraContext
    {
        Establish context = () =>
        {
            streamService.Attach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("duplicate" + sandbox));
            streamService.Attach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("duplicate" + sandbox));
        };

        Because of = () => streamService.Attach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("duplicate" + sandbox));

        It should_be_attached_only_once = () => streamService.Get(Encoding.UTF8.GetBytes("top" + sandbox)).Streams.Count().ShouldEqual(1);
    }
}
