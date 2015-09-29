using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Feeds
{
    [Subject("Feeds")]
    public class When_feeds_and_streams_are_attached_and_detached
    {
        Establish context = () =>
        {

            var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));

            var feed1 = feedFactory.GG(Encoding.UTF8.GetBytes("feed1"));
            feed1.Attach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("this")));

            var feed2 = feedFactory.GG(Encoding.UTF8.GetBytes("feed2"));
            feed2.Attach(new Stream(feed2.FeedId, Encoding.UTF8.GetBytes("is")));

            var feed3 = feedFactory.GG(Encoding.UTF8.GetBytes("feed3"));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("a")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("normal")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("test")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("with")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("some")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("idk")));
            feed3.Attach(new Stream(feed3.FeedId, Encoding.UTF8.GetBytes("what")));


            var feed4 = feedFactory.GG(Encoding.UTF8.GetBytes("feed4"));
            feed4.Attach(new Stream(feed4.FeedId, Encoding.UTF8.GetBytes("again")));
            feed4.Attach(new Stream(feed4.FeedId, Encoding.UTF8.GetBytes("test")));

            var feed5 = feedFactory.GG(Encoding.UTF8.GetBytes("feed5"));
            feed5.Attach(new Stream(feed5.FeedId, Encoding.UTF8.GetBytes("another one")));
            feed5.Attach(new Stream(feed5.FeedId, Encoding.UTF8.GetBytes("guys")));

            feed1.Attach(feed3); //1 stream, 1 feed

            feed2.Attach(feed3); //1 stream, 1 feed
            feed2.Attach(feed4); //1 stream, 2 feeds

            feed1.Attach(feed2); //1 stream, 2 feeds
            feed1.Attach(feed5); //1 stream, 3 feeds

            feed1.Detach(feed2); //1 stream, 2 feeds

            feed1.Attach(new Stream(feed1.FeedId, Encoding.UTF8.GetBytes("begone"))); //2 streams, 2 feeds(feed3, feed5)


            feed = feed1;
            //var result = feed1.Streams;
        };

        //Because of = () => feed.Attach(new FeedStream(feed.Id, secondStreamId));

        It should_return_streams_count_without_the_detached_ones = () => feed.Streams.Count().ShouldEqual(11);

        static IStream feed;
    }
}

