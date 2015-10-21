using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Elders.Web.Api;
using System;

namespace ActivityStreams.Api.Controllers
{
    public class ActivitiesController : ApiController
    {
        /// <summary>
        ///  Post activity to stream
        /// </summary>
        [HttpPost]
        public IHttpActionResult PostActivity(PostActivityModel model)
        {
            var activityIdBytes = Encoding.UTF8.GetBytes(model.ActivityId);
            var streamIdBytes = Encoding.UTF8.GetBytes(model.StreamId);

            var activity = new Activity(streamIdBytes, activityIdBytes, model.Body, null);
            WebApiApplication.ActivityRepository.Append(activity);
            return this.Ok(model.ActivityId);
        }
        /// <summary>
        /// Load Activities for feed
        /// </summary>
        /// <param name="feedId"></param>
        /// /// <param name="feedOptions"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<FeedModel> LoadActivities(string feedId, FeedOptions feedOptions)
        {
            feedOptions = feedOptions ?? FeedOptions.Default;

            var feedIdBytes = Encoding.UTF8.GetBytes(feedId);

            var feed = ActivityStreams.Api.WebApiApplication.FeedFactory.GG(feedIdBytes);
            if (feed == null)
                return new ResponseResult<FeedModel>();

            var activities = WebApiApplication.ActivityRepository.Load(feed, feedOptions);
            return new ResponseResult<FeedModel>(new FeedModel(activities));
        }

        public class PostActivityModel
        {
            public string ActivityId { get; set; }

            public string StreamId { get; set; }

            public object Body { get; set; }
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
