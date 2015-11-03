//using System;
//using System.Text;
//using Machine.Specifications;

//namespace ActivityStreams.Tests.Feeds
//{
//    [Subject("Feeds")]
//    public class When_closing_a_feed_which_is_already_closed : InMemoryContext
//    {
//        Establish context = () =>
//            {
//                var feedId = Encoding.UTF8.GetBytes("ownerId");
//                var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));
//                feed = feedFactory.Get(feedId);
//                var firstStreamId = Encoding.UTF8.GetBytes("streamId");
//                feed.Attach(firstStreamId);
//                feed.Close(DateTime.UtcNow);
//            };

//        Because of = () => Exception = Catch.Exception(() => feed.Close(DateTime.UtcNow));

//        It should_fail = () => Exception.ShouldNotBeNull();
//        It should_have_a_specific_reason = () => Exception.Message.ShouldContain("already closed");

//        static IStream feed;
//        static Exception Exception;
//    }
//}
