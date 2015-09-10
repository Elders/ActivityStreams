using System.Web.Http;
using ActivityStreams.Persistence;

namespace ActivityStreams.Api.Controllers
{
    public class FeedsController : ApiController
    {
        IActivityFeedRepository ActivityFeedRepository;

        public IHttpActionResult AttachStream(byte[] feedId, byte[] streamId)
        {
            var feed = ActivityFeedRepository.Get(feedId);
            if (feed == null) return base.NotFound();

            var feedStream = new FeedStream(feedId, streamId);
            feed.AttachStream(feedStream);
            ActivityFeedRepository.Save(feed);

            return this.Ok();
        }

        public IHttpActionResult DetachStream(byte[] feedId, byte[] streamId)
        {
            var feed = ActivityFeedRepository.Get(feedId);
            if (feed == null) return base.NotFound();

            var feedStream = new FeedStream(feedId, streamId);
            feed.DetachStream(feedStream);
            ActivityFeedRepository.Save(feed);

            return this.Ok();
        }
    }
}
