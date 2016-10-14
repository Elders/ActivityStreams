using System;
using System.Collections.Generic;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Persistence.Cassandra.Tests.Activities
{

    [Subject("StreamCrawler")]
    public class When_crawling_streams_and_a_stream_is_attached_twice_trough_multilevel_attachments : CassandraContext
    {
        Establish context = () =>
            {
                crawler = new StreamCrawler(activityStreamStore);

                Q = Encoding.UTF8.GetBytes("Q" + sandbox);
                X = Encoding.UTF8.GetBytes("X" + sandbox);
                P = Encoding.UTF8.GetBytes("P" + sandbox);
                L = Encoding.UTF8.GetBytes("L" + sandbox);

                streamService.Attach(Q, X);
                streamService.Attach(Q, P);
                streamService.Attach(P, L);
                streamService.Attach(L, X);
                streamService.Attach(X, Q);

                streamService.Detach(Q, X, new DateTime(2000, 01, 3));

                stream = streamService.Get(Q);
            };

        Because of = () => result = crawler.StreamsToLoad(stream, executionTimestamp);

        It should_have_proper__Q__stream_timestamp_for_loading_activities = () => result[Q].ShouldEqual(executionTimestamp);
        It should_have_proper__X__stream_timestamp_for_loading_activities = () => result[X].ShouldEqual(executionTimestamp);
        It should_have_proper__P__stream_timestamp_for_loading_activities = () => result[P].ShouldEqual(executionTimestamp);
        It should_have_proper__L__stream_timestamp_for_loading_activities = () => result[L].ShouldEqual(executionTimestamp);

        static ActivityStream stream;
        static StreamCrawler crawler;
        static Dictionary<byte[], long> result;
        static long executionTimestamp = new DateTime(2001, 1, 1).ToFileTimeUtc();
        static byte[] Q;
        static byte[] X;
        static byte[] P;
        static byte[] L;
    }
}