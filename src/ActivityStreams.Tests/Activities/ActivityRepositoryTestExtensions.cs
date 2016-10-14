using System;
using System.Text;
using ActivityStreams.Persistence;

namespace ActivityStreams.Tests.Activities
{

    public static class ActivityRepositoryTestExtensions
    {
        public static Activity NewActivity(this IActivityRepository activityRepository, byte[] streamId, int pattern)
        {
            var id = Encoding.UTF8.GetBytes($"activity_{pattern}");
            var activity = new Activity(streamId, id, $"body_{pattern}", $"author_{pattern}", new DateTime(2000, 1, pattern));
            activityRepository.Append(activity);
            return activity;
        }
    }
}