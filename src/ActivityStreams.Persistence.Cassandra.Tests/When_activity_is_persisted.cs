using System.Reflection;
using System.Text;
using Cassandra;
using Machine.Specifications;
using System;
using System.IO;
using ActivityStreams.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_activity_is_persisted
    {
        Establish context = () =>
        {
            session = SessionCreator.Create();
            serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            var store = new ActivityStore(session, serializer);
            activityRepository = new ActivityRepository(store);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateActivitiesStorage();

            var shit = DateTime.UtcNow;
            timestamp = shit.ToFileTimeUtc();
            streamId = Encoding.UTF8.GetBytes("streamId" + timestamp);
            externalId = Encoding.UTF8.GetBytes("activityId");
            body = new ActivityBody() { Content = "test content" };
            activity = new Activity(streamId, externalId, body, "author", shit);
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

        static ISerializer serializer;
        static ISession session;
        static IActivityRepository activityRepository;
        static Activity activity;
    }
}
