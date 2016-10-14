using System;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_stream_is_detached : InMemoryContext
    {
        Establish context = () =>
        {
            streamService.Attach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("attached-level_1" + sandbox));
        };

        Because of = () => streamService.Detach(Encoding.UTF8.GetBytes("top" + sandbox), Encoding.UTF8.GetBytes("attached-level_1" + sandbox), DateTime.UtcNow);

        It should_not_remove_the_detached_stream = () => streamService.Get(Encoding.UTF8.GetBytes("top" + sandbox)).Streams.Count().ShouldEqual(1);
        It should_have_timestamp_when_detach_happened;
    }
}

