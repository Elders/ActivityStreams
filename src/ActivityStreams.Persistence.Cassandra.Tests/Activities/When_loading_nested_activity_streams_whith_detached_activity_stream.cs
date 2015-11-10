using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_nested_activity_streams_NEW : CassandraContext
    {
        Establish context = () =>
            {
                var X = Encoding.UTF8.GetBytes("X" + sandbox);
                act_1 = activityRepository.NewActivity(X, 1);
                act_3 = activityRepository.NewActivity(X, 3);
                act_5 = activityRepository.NewActivity(X, 5);
                act_7 = activityRepository.NewActivity(X, 7);
                act_9 = activityRepository.NewActivity(X, 9);

                var Y = Encoding.UTF8.GetBytes("Y" + sandbox);
                act_2 = activityRepository.NewActivity(Y, 2);
                act_4 = activityRepository.NewActivity(Y, 4);
                act_6 = activityRepository.NewActivity(Y, 6);
                act_8 = activityRepository.NewActivity(Y, 8);
                act_10 = activityRepository.NewActivity(Y, 10);

                var Z = Encoding.UTF8.GetBytes("Z" + sandbox);
                act_22 = activityRepository.NewActivity(Z, 22);
                act_24 = activityRepository.NewActivity(Z, 24);
                act_26 = activityRepository.NewActivity(Z, 26);
                act_28 = activityRepository.NewActivity(Z, 28);
                act_30 = activityRepository.NewActivity(Z, 30);

                var Q = Encoding.UTF8.GetBytes("Q" + sandbox);
                act_11 = activityRepository.NewActivity(Q, 11);
                act_13 = activityRepository.NewActivity(Q, 13);
                act_15 = activityRepository.NewActivity(Q, 15);
                act_17 = activityRepository.NewActivity(Q, 17);
                act_19 = activityRepository.NewActivity(Q, 19);

                var P = Encoding.UTF8.GetBytes("P" + sandbox);
                act_12 = activityRepository.NewActivity(P, 12);
                act_14 = activityRepository.NewActivity(P, 14);
                act_16 = activityRepository.NewActivity(P, 16);
                act_18 = activityRepository.NewActivity(P, 18);
                act_20 = activityRepository.NewActivity(P, 20);

                var L = Encoding.UTF8.GetBytes("L" + sandbox);
                act_21 = activityRepository.NewActivity(L, 21);
                act_23 = activityRepository.NewActivity(L, 23);
                act_25 = activityRepository.NewActivity(L, 25);
                act_27 = activityRepository.NewActivity(L, 27);
                act_29 = activityRepository.NewActivity(L, 29);

                streamService.Attach(Q, X);
                streamService.Attach(Q, Y);
                streamService.Attach(Q, Z);
                streamService.Attach(Q, P);
                streamService.Attach(P, L);
                streamService.Attach(L, X);
                streamService.Attach(L, Y);

                stream = streamService.Get(Q);
            };

        Because of = () => activityStream = activityRepository.Load(stream, ActivityStreamOptions.Default).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(30);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(act_1);
            activityStream[1].ShouldEqual(act_2);
            activityStream[2].ShouldEqual(act_3);
            activityStream[3].ShouldEqual(act_4);
            activityStream[4].ShouldEqual(act_5);
            activityStream[5].ShouldEqual(act_6);
            activityStream[6].ShouldEqual(act_7);
            activityStream[7].ShouldEqual(act_8);
            activityStream[8].ShouldEqual(act_9);
            activityStream[9].ShouldEqual(act_10);
            activityStream[10].ShouldEqual(act_11);
            activityStream[11].ShouldEqual(act_12);
            activityStream[12].ShouldEqual(act_13);
            activityStream[13].ShouldEqual(act_14);
            activityStream[14].ShouldEqual(act_15);
            activityStream[15].ShouldEqual(act_16);
            activityStream[16].ShouldEqual(act_17);
            activityStream[17].ShouldEqual(act_18);
            activityStream[18].ShouldEqual(act_19);
            activityStream[19].ShouldEqual(act_20);
            activityStream[20].ShouldEqual(act_21);
            activityStream[21].ShouldEqual(act_22);
            activityStream[22].ShouldEqual(act_23);
            activityStream[23].ShouldEqual(act_24);
            activityStream[24].ShouldEqual(act_25);
            activityStream[25].ShouldEqual(act_26);
            activityStream[26].ShouldEqual(act_27);
            activityStream[27].ShouldEqual(act_28);
            activityStream[28].ShouldEqual(act_29);
            activityStream[29].ShouldEqual(act_30);
        };

        static ActivityStream stream;
        static List<Activity> activityStream;
        static Activity act_1, act_2, act_3, act_4, act_5, act_6, act_7, act_8, act_9, act_10;
        static Activity act_11, act_12, act_13, act_14, act_15, act_16, act_17, act_18, act_19, act_20;
        static Activity act_21, act_22, act_23, act_24, act_25, act_26, act_27, act_28, act_29, act_30;
    }
}
