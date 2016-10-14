using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_nested_activity_streams : InMemoryContext
    {
        Establish context = () =>
            {
                var streamId1 = Encoding.UTF8.GetBytes("streamId1" + sandbox);
                act_a1 = activityRepository.NewActivity(streamId1, 1);
                act_a3 = activityRepository.NewActivity(streamId1, 3);
                act_a5 = activityRepository.NewActivity(streamId1, 5);
                act_a7 = activityRepository.NewActivity(streamId1, 7);
                act_a9 = activityRepository.NewActivity(streamId1, 9);

                var streamId2 = Encoding.UTF8.GetBytes("streamId2" + sandbox);
                act_a2 = activityRepository.NewActivity(streamId2, 2);
                act_a4 = activityRepository.NewActivity(streamId2, 4);
                act_a6 = activityRepository.NewActivity(streamId2, 6);
                act_a8 = activityRepository.NewActivity(streamId2, 8);
                act_a10 = activityRepository.NewActivity(streamId2, 10);

                streamService.Attach(streamId1, streamId2);

                stream = streamService.Get(streamId1);
            };

        Because of = () => activityStream = activityRepository.Load(stream, ActivityStreamOptions.Default).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(10);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(act_a1);
            activityStream[1].ShouldEqual(act_a2);
            activityStream[2].ShouldEqual(act_a3);
            activityStream[3].ShouldEqual(act_a4);
            activityStream[4].ShouldEqual(act_a5);
            activityStream[5].ShouldEqual(act_a6);
            activityStream[6].ShouldEqual(act_a7);
            activityStream[7].ShouldEqual(act_a8);
            activityStream[8].ShouldEqual(act_a9);
            activityStream[9].ShouldEqual(act_a10);
        };

        static ActivityStream stream;
        static List<Activity> activityStream;
        static Activity act_a1, act_a2, act_a3, act_a4, act_a5, act_a6, act_a7, act_a8, act_a9, act_a10;
    }
}
