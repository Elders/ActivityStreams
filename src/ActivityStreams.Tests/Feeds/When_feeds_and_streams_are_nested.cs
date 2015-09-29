using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_feeds_and_streams_are_nested
    {
        Establish context = () =>
        {

            var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));

            var feed1 = feedFactory.GG(Encoding.UTF8.GetBytes("feed1"));
            feed1.Attach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("this")));
            feed1.Attach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("that")));
            feed1.Attach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("begone")));
            feed1.Detach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("begone")));

            var feed2 = feedFactory.GG(Encoding.UTF8.GetBytes("feed2"));
            feed2.Attach(new Stream(feed2.FeedId, Encoding.UTF8.GetBytes("is")));

            feed1.Attach(feed2);

            var feed3 = feedFactory.GG(Encoding.UTF8.GetBytes("feed3"));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("tova")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("onova")));

            feed1.Attach(feed3);
            feed1.Detach(feed2);

            feed = feed1;
        };

        It should_return_all_nested_streams_without_duplicates = () => feed.Streams.Count().ShouldEqual(4);

        static IStream feed;
    }
}

