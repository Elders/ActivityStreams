using System;
using System.Text;
using System.Web.Http;

namespace ActivityStreams.Api.Controllers
{
    /// <summary>
    /// Stream end point
    /// </summary>
    public class StreamController : ApiController
    {
        /// <summary>
        /// Attach stream to stream
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="toStreamId"></param>
        /// <param name="expiresAt">Expiration date and time. Default is DateTime.MaxValue</param>
        /// <returns>IHttpActionResult</returns>
        /// <remarks>Attaching stream to stream</remarks>
        [HttpPost]
        public IHttpActionResult AttachStream(string streamId, string toStreamId, DateTime? expiresAt = null)
        {
            if (expiresAt.HasValue == false)
                expiresAt = DateTime.FromFileTimeUtc(ActivityStream.DefaultExpirationTimestamp);


            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);
            var toStreamIdBytes = Encoding.UTF8.GetBytes(toStreamId);
            WebApiApplication.StreamService.Attach(toStreamIdBytes, streamIdBytes, expiresAt.Value);

            return Ok();
        }

        /// <summary>
        /// Detach stream from stream
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="fromStreamId"></param>
        /// <param name="detachSince">Detach date and time</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DetachStream(string streamId, string fromStreamId, DateTime detachSince)
        {
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);
            var fromStreamIdBytes = Encoding.UTF8.GetBytes(fromStreamId);
            WebApiApplication.StreamService.Detach(fromStreamIdBytes, streamIdBytes, detachSince);

            return Ok();
        }
    }
}
