using System;
using System.Text;
using System.Runtime.Serialization;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    public static class ActivityRepositoryTestExtensions
    {
        public static Activity NewActivity(this IActivityRepository activityRepository, byte[] streamId, int pattern)
        {
            var id = Encoding.UTF8.GetBytes($"activity_{pattern}");
            var activity = new Activity(streamId, id, new TestActivityBody($"body_{pattern}"), $"author_{pattern}", new DateTime(2000, 1, pattern));
            activityRepository.Append(activity);
            return activity;
        }
    }

    [DataContract(Name = "3705d2fb-e501-4167-89f9-67bd654927e0")]
    public class TestActivityBody
    {
        TestActivityBody() { }

        public TestActivityBody(string body)
        {
            Body = body;
        }

        [DataMember(Order = 1)]
        public string Body { get; set; }
    }
}
