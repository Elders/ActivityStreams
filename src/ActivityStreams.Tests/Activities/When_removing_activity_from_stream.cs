using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_removing_activity_from_stream : InMemoryContext
    {
        Establish context = () =>
        {
            var streamId = Encoding.UTF8.GetBytes("streamId" + sandbox);

            act_1 = activityRepository.NewActivity(streamId, 1);
            act_2 = activityRepository.NewActivity(streamId, 2);
            act_3 = activityRepository.NewActivity(streamId, 3);

            stream = streamService.Get(streamId);
        };

        Because of = () => activityRepository.Remove(act_2.StreamId, act_2.Timestamp);

        It should_return_all_not_removed_activities = () => activityRepository.Load(stream, ActivityStreamOptions.Default).ToList().Count.ShouldEqual(2);

        It should_not_contain_removed_activity = () =>
        {
            activityRepository.Load(stream, ActivityStreamOptions.Default).ToList().ShouldNotContain(act_2);
        };

        It should_contain_only_not_removed_activities = () =>
        {
            activityRepository.Load(stream, ActivityStreamOptions.Default).ToList().ShouldContain(act_1, act_3);
        };

        static ActivityStream stream;
        static Activity act_1;
        static Activity act_2;
        static Activity act_3;
    }
}

