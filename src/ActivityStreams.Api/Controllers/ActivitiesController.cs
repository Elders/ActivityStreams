﻿using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Elders.Web.Api;

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

            var activity = new Activity(streamIdBytes, activityIdBytes, model.Body, string.Empty);
            WebApiApplication.ActivityRepository.Append(activity);

            return Ok(model.ActivityId);
        }
        /// <summary>
        /// Load Activities for stream
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<StreamModel> LoadActivities(string streamId, ActivityStreamOptions options)
        {
            options = options ?? ActivityStreamOptions.Default;

            var streamIdBytes = Encoding.UTF8.GetBytes(streamId);
            var stream = WebApiApplication.StreamService.Get(streamIdBytes);

            var activities = WebApiApplication.ActivityRepository.Load(stream, options);
            return new ResponseResult<StreamModel>(new StreamModel(activities));
        }

        public class PostActivityModel
        {
            public string ActivityId { get; set; }

            public string StreamId { get; set; }

            public object Body { get; set; }
        }
    }

    public class StreamModel
    {
        public StreamModel(IEnumerable<Activity> activities)
        {
            Activities = activities;
        }

        public IEnumerable<Activity> Activities { get; set; }
    }
}
