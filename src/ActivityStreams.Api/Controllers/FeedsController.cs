using System.Text;
using System.Web.Http;

namespace ActivityStreams.Api.Controllers
{
    public class FeedsController : ApiController
    {
        public IHttpActionResult AttachStream(string feedId, string streamId)
        {
            var feedIdBytes = Encoding.UTF8.GetBytes(feedId);
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);

            var feed = WebApiApplication.FeedRepository.Get(feedIdBytes);
            if (feed == null) return base.NotFound();

            var feedStream = new FeedStream(feedIdBytes, streamIdBytes);
            feed.AttachStream(feedStream);
            WebApiApplication.FeedRepository.Save(feed);

            return this.Ok();
        }

        public IHttpActionResult DetachStream(string feedId, string streamId)
        {
            var feedIdBytes = Encoding.UTF8.GetBytes(feedId);
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);

            var feed = WebApiApplication.FeedRepository.Get(feedIdBytes);
            if (feed == null) return base.NotFound();

            var feedStream = new FeedStream(feedIdBytes, streamIdBytes);
            feed.DetachStream(feedStream);
            WebApiApplication.FeedRepository.Save(feed);

            return this.Ok();
        }
    }
}
