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
            streamService.Attach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("attached-level_1"));
        };

        Because of = () => streamService.Detach(Encoding.UTF8.GetBytes("top"), Encoding.UTF8.GetBytes("attached-level_1"), DateTime.UtcNow);

        It should_not_remove_the_detached_stream = () => streamService.Get(Encoding.UTF8.GetBytes("top")).Streams.Count().ShouldEqual(2);
        It should_have_timestamp_when_detach_happened;
    }
}

