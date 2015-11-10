using System;
using System.Linq;
using System.Text;
using ActivityStreams.Helpers;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_feed_is_persisted : CassandraContext
    {
        Establish context = () =>
        {
            var capturedTimestamp = DateTime.UtcNow;
            timestamp = capturedTimestamp.ToFileTimeUtc();
            streamId = Encoding.UTF8.GetBytes("top23" + timestamp);
            stream = streamService.Get(streamId);
            attachedId = Encoding.UTF8.GetBytes("asid" + timestamp);
        };

        Because of = () => streamService.Attach(streamId, attachedId);

        It should_have_the_attached_feed_stream = () =>
        {
            var stream = streamService.Get(streamId);
            stream.Streams.Count().ShouldEqual(1);
            stream.AttachedStreams.Any(x => ByteArrayHelper.Compare(x.StreamId, attachedId)).ShouldBeTrue();
        };

        static byte[] streamId;
        static byte[] attachedId;
        static ActivityStream stream;
        static long timestamp;
    }
}
