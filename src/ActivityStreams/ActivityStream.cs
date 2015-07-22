using System.Collections.Generic;
using System.Linq;

namespace ActivityStreams
{
    public class ActivityStream
    {
        readonly HashSet<Activity> activities;

        public ActivityStream()
        {
            activities = new HashSet<Activity>();
        }

        public void Append(Activity item)
        {
            activities.Add(item);
        }

        public IEnumerable<Activity> Activities { get { return activities.ToList().AsReadOnly(); } }
    }
}