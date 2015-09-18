using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_single_activity_stream
    {
        Establish context = () =>
            {
                var streamId = Encoding.UTF8.GetBytes("streamId");

                var id1 = Encoding.UTF8.GetBytes("activityId1");
                item1 = Activity.UnitTestFactory(id1, streamId, "body1", "author1", DateTime.UtcNow.AddMinutes(1));

                var id2 = Encoding.UTF8.GetBytes("activityId2");
                item2 = Activity.UnitTestFactory(id2, streamId, "body2", "author2", DateTime.UtcNow.AddMinutes(2));

                var id3 = Encoding.UTF8.GetBytes("activityId3");
                item3 = Activity.UnitTestFactory(id3, streamId, "body3", "author3", DateTime.UtcNow.AddMinutes(3));

                activityStreamRepository = new InMemoryActivityRepository();
                activityStreamRepository.Append(item2);
                activityStreamRepository.Append(item3);
                activityStreamRepository.Append(item1);

                var subscriptionOwnerId = Encoding.UTF8.GetBytes("subscriptionOwnerId");
                subscription = new Feed(subscriptionOwnerId);
                subscription.AttachStream(new FeedStream(subscriptionOwnerId, streamId));
            };

        Because of = () => activityStream = activityStreamRepository.Load(subscription).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(3);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(item1);
            activityStream[1].ShouldEqual(item2);
            activityStream[2].ShouldEqual(item3);
        };

        static IActivityRepository activityStreamRepository;
        static Feed subscription;
        static List<Activity> activityStream;
        static Activity item1;
        static Activity item2;
        static Activity item3;
    }
}
