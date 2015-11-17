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
        /// <returns>IHttpActionResult</returns>
        /// <remarks>Attaching stream to stream</remarks>
        [HttpPost]
        public IHttpActionResult AttachStream(string streamId, string toStreamId)
        {
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);
            var toStreamIdBytes = Encoding.UTF8.GetBytes(toStreamId);
            WebApiApplication.StreamService.Attach(toStreamIdBytes, streamIdBytes);

            return Ok();
        }

        /// <summary>
        /// Attach stream to stream with expiration date
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="toStreamId"></param>
        /// <param name="expiresAt"></param>
        /// <returns>IHttpActionResult</returns>
        /// <remarks>Attaching stream to stream with expiration date</remarks>
        [HttpPost]
        public IHttpActionResult AttachStreamWithExpirationDate(string streamId, string toStreamId, DateTime expiresAt)
        {
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);
            var toStreamIdBytes = Encoding.UTF8.GetBytes(toStreamId);
            WebApiApplication.StreamService.Attach(toStreamIdBytes, streamIdBytes, expiresAt);

            return Ok();
        }

        /// <summary>
        /// Detach stream from stream
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="fromStreamId"></param>
        /// <param name="detachSince"></param>
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
