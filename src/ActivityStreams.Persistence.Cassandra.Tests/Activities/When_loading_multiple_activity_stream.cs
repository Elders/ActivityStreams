using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_multiple_activity_stream : CassandraContext
    {
        Establish context = () =>
            {
                var streamId1 = Encoding.UTF8.GetBytes("streamId1" + sandbox);
                var streamId2 = Encoding.UTF8.GetBytes("streamId2" + sandbox);

                act_1 = activityRepository.NewActivity(streamId1, 1);
                act_2 = activityRepository.NewActivity(streamId2, 2);
                act_3 = activityRepository.NewActivity(streamId1, 3);


                var streamId = Encoding.UTF8.GetBytes("subscriptionOwnerId" + sandbox);
                streamService.Attach(streamId, streamId1);
                streamService.Attach(streamId, streamId2);
                stream = streamService.Get(streamId);
            };

        Because of = () => activityStream = activityRepository.Load(stream, ActivityStreamOptions.Default).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(3);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(act_1);
            activityStream[1].ShouldEqual(act_2);
            activityStream[2].ShouldEqual(act_3);
        };

        static ActivityStream stream;
        static List<Activity> activityStream;
        static Activity act_1;
        static Activity act_2;
        static Activity act_3;
    }
}
