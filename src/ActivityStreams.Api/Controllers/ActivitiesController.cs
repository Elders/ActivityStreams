using System.Collections.Generic;
using System.Web.Http;
using ActivityStreams.Persistence;
using Elders.Web.Api;

namespace ActivityStreams.Api.Controllers
{
    public class ActivitiesController : ApiController
    {
        IActivityRepository ActivityRepository;
        IActivityFeedRepository FeedRepository;

        public IHttpActionResult PostActivity(byte[] activityId, byte[] streamId, object body)
        {
            var activity = new Activity(activityId, streamId, body, null);
            ActivityRepository.Append(activity);
            return this.Accepted(streamId);
        }

        public ResponseResult<FeedModel> LoadActivities(byte[] feedId)
        {

            var feed = FeedRepository.Get(feedId);
            var activities = ActivityRepository.Load(feed);

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
