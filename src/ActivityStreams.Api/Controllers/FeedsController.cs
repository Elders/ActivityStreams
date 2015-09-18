using System.Text;
using System.Web.Http;

namespace ActivityStreams.Api.Controllers
{
    /// <summary>
    /// Feeds end point
    /// </summary>
    public class FeedsController : ApiController
    {
        /// <summary>
        /// Attach stream to feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="streamId"></param>
        /// <returns>IHttpActionResult</returns>
        /// <remarks>Attaching stream to feed</remarks>
        [HttpPost]
        public IHttpActionResult AttachStream(string feedId, string streamId)
        {
            var feedIdBytes = Encoding.UTF8.GetBytes(feedId);
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);

            var feed = WebApiApplication.FeedRepository.Get(feedIdBytes);
            if (feed == null)
            {
                feed = new Feed(feedIdBytes);
            }

            var feedStream = new FeedStream(feedIdBytes, streamIdBytes);
            feed.AttachStream(feedStream);
            WebApiApplication.FeedRepository.Save(feed);

            return this.Ok();
        }

        /// <summary>
        /// Detach stream from feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="streamId"></param>
        /// <returns></returns>
        [HttpPost]
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
