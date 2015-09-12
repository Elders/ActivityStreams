using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Elders.Web.Api;

namespace ActivityStreams.Api.Controllers
{
    /// <summary>
    /// Activities end point
    /// </summary>
    public class ActivitiesController : ApiController
    {
        /// <summary>
        ///  Post activity to stream
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="streamId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostActivity(string activityId, string streamId, object body)
        {
            var activityIdBytes = Encoding.UTF8.GetBytes(activityId);
            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);

            var activity = new Activity(activityIdBytes, streamIdBytes, body, null);
            WebApiApplication.ActivityRepository.Append(activity);
            return this.Accepted(streamId);
        }
        /// <summary>
        /// Load Activities for feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<FeedModel> LoadActivities(string feedId)
        {
            var feedIdBytes = Encoding.UTF8.GetBytes(feedId);

            var feed = WebApiApplication.FeedRepository.Get(feedIdBytes);
            var activities = WebApiApplication.ActivityRepository.Load(feed);

            return new ResponseResult<FeedModel>(new FeedModel(activities));
        }
    }

    public class FeedModel
    {
        public FeedModel(IEnumerable<Activity> activities)
        {
            Activities = activities;
        }

        public IEnumerable<Activity> Activities { get; set; }
    }
}
