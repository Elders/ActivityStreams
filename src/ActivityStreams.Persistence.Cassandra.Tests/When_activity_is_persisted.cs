using System;
using System.IO;
using System.Text;
using ActivityStreams.Helpers;
using Machine.Specifications;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_activity_is_persisted : CassandraContext
    {
        Establish context = () =>
        {
            var capturedTimestamp = DateTime.UtcNow;

            timestamp = capturedTimestamp.ToFileTimeUtc();
            streamId = Encoding.UTF8.GetBytes("streamId" + timestamp);
            externalId = Encoding.UTF8.GetBytes("activityId");
            body = new ActivityBody() { Content = "test content" };
            activity = new Activity(streamId, externalId, body, "author", capturedTimestamp);
        };

        Because of = () => activityRepository.Append(activity);

        It should_have_the_activity_stored_properly = () =>
        {
            var data = session.GetBytesById(streamId);

            ByteArrayHelper.Compare(serializer.SerializeToBytes(activity), data).ShouldBeTrue();
            using (var stream = new MemoryStream(data))
            {
                var storedActivity = (Activity)serializer.Deserialize(stream);
                ByteArrayHelper.Compare(storedActivity.StreamId, streamId).ShouldBeTrue();
                storedActivity.Timestamp.ShouldEqual(timestamp);
                ByteArrayHelper.Compare(storedActivity.ExternalId, externalId).ShouldBeTrue();
            }
        };

        static byte[] streamId;
        static long timestamp;
        static byte[] externalId;
        static object body;

        static Activity activity;
    }
}
