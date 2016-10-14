using System;
using System.Collections.Generic;
using System.Text;
using ActivityStreams.Persistence;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{

    [Subject("StreamCrawler")]
    public class When_crawling_streams_without_detachments : InMemoryContext
    {
        Establish context = () =>
            {
                crawler = new StreamCrawler(activityStreamStore);

                var X = Encoding.UTF8.GetBytes("X" + sandbox);
                var Q = Encoding.UTF8.GetBytes("Q" + sandbox);
                var P = Encoding.UTF8.GetBytes("P" + sandbox);
                var L = Encoding.UTF8.GetBytes("L" + sandbox);

                streamService.Attach(Q, X);
                streamService.Attach(Q, P);
                streamService.Attach(P, L);
                streamService.Attach(L, X);
                streamService.Attach(X, Q);

                stream = streamService.Get(Q);
            };

        Because of = () => result = crawler.StreamsToLoad(stream, executionTimestamp);

        It should_proper_timestamps_for_loading_activities = () =>
        {
            foreach (var str in result.Values)
            {
                str.ShouldEqual(executionTimestamp);
            }
        };

        static ActivityStream stream;
        static StreamCrawler crawler;
        static Dictionary<byte[], long> result;
        static long executionTimestamp = new DateTime(2001, 1, 1).ToFileTimeUtc();
    }
}