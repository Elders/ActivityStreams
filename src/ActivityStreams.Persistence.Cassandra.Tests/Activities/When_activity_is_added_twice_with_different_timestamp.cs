using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Activities
{
    [Subject("Streams")]
    public class When_activity_is_added_twice_with_different_timestamp : CassandraContext
    {
        Establish context = () =>
        {
            var streamId = Encoding.UTF8.GetBytes("streamId" + sandbox);
            var activityId = Encoding.UTF8.GetBytes("activityId" + sandbox);

            act_1 = activityRepository.NewActivity(streamId, activityId, 1);
            act_2 = activityRepository.NewActivity(streamId, activityId, 2);

            stream = streamService.Get(streamId);
        };

        Because of = () => activityStream = activityRepository.Load(stream, ActivityStreamOptions.Default).ToList();

        It should_return_only_one_activity = () => activityStream.Count.ShouldEqual(1);

        It should_return_only_one_activiasdty = () => activityStream.Single().Timestamp.ShouldEqual(act_2.Timestamp);

        static ActivityStream stream;
        static List<Activity> activityStream;
        static Activity act_1;
        static Activity act_2;
    }
}
