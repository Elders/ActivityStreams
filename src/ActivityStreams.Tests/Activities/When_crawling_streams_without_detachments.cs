using System;
using System.Collections.Generic;
using System.Linq;
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

                var X = Encoding.UTF8.GetBytes("X");
                var Q = Encoding.UTF8.GetBytes("Q");
                var P = Encoding.UTF8.GetBytes("P");
                var L = Encoding.UTF8.GetBytes("L");

                streamService.Attach(Q, X);
                streamService.Attach(Q, P);
                streamService.Attach(P, L);
                streamService.Attach(L, X);
                streamService.Attach(X, Q);

                //streamService.Detach(Q, X, new DateTime(2000, 01, 3));
                //streamService.Detach(Q, P, new DateTime(2000, 01, 13));
                //streamService.Detach(P, L, new DateTime(2000, 01, 10));
                //streamService.Detach(L, X, new DateTime(2000, 01, 15));

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