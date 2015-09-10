using System;
using System.Text;
using System.Web.Http;
using ActivityStreams.Persistence;
using Elders.Web.Api;
using System.Collections.Generic;

namespace ActivityStreams.Api.Controllers
{
    public class ActivitiesController : ApiController
    {
        IActivityFeedRepository ActivityFeedRepository;
        ISubscriptionRepository SubscriptionsRepository;

        public IHttpActionResult PostActivity(byte[] streamId, object body)
        {
            var activity = new Activity(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()), streamId, body, null);

            ActivityFeedRepository.Append(activity);

            return this.Accepted(streamId);
        }

        public ResponseResult<FeedModel> GetActivityFeed(byte[] ownerId)
        {
            var sub = SubscriptionsRepository.LoadOwnerSubscription(ownerId);

            var feed = ActivityFeedRepository.Load(sub);

            return new ResponseResult<FeedModel>(new FeedModel(feed));
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
