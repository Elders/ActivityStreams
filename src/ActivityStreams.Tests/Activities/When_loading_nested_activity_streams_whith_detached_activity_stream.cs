using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_nested_activity_streams_whith_detached_activity_stream : InMemoryContext
    {
        Establish context = () =>
            {
                var streamId1 = Encoding.UTF8.GetBytes("streamId1");
                act_a1 = NewActivity(streamId1, 1);
                act_a3 = NewActivity(streamId1, 3);
                act_a5 = NewActivity(streamId1, 5);
                act_a7 = NewActivity(streamId1, 7);
                act_a9 = NewActivity(streamId1, 9);
                activityRepository.Append(act_a1);
                activityRepository.Append(act_a3);
                activityRepository.Append(act_a5);
                activityRepository.Append(act_a7);
                activityRepository.Append(act_a9);

                var streamId2 = Encoding.UTF8.GetBytes("streamId2");
                act_a2 = NewActivity(streamId2, 2);
                act_a4 = NewActivity(streamId2, 4);
                act_a6 = NewActivity(streamId2, 6);
                act_a8 = NewActivity(streamId2, 8);
                act_a10 = NewActivity(streamId2, 10);
                activityRepository.Append(act_a2);
                activityRepository.Append(act_a4);
                activityRepository.Append(act_a6);
                activityRepository.Append(act_a8);
                activityRepository.Append(act_a10);

                streamService.Attach(streamId1, streamId2);
                streamService.Detach(streamId1, streamId2, new DateTime(2000, 1, 7));
                streamService.Detach(streamId1, streamId1, new DateTime(2000, 1, 8));

                stream = streamService.Get(streamId1);
            };

        Because of = () => activityStream = activityRepository.Load(stream, ActivityStreamOptions.Default).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(7);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(act_a1);
            activityStream[1].ShouldEqual(act_a2);
            activityStream[2].ShouldEqual(act_a3);
            activityStream[3].ShouldEqual(act_a4);
            activityStream[4].ShouldEqual(act_a5);
            activityStream[5].ShouldEqual(act_a6);
            activityStream[6].ShouldEqual(act_a7);
        };

        static ActivityStream stream;
        static List<Activity> activityStream;
        static Activity act_a1, act_a2, act_a3, act_a4, act_a5, act_a6, act_a7, act_a8, act_a9, act_a10;

        static Activity NewActivity(byte[] streamId, int pattern)
        {
            var id = Encoding.UTF8.GetBytes($"activity_{pattern}");
            return new Activity(streamId, id, $"body_{pattern}", $"author_{pattern}", new DateTime(2000, 1, pattern));
        }
    }
}
