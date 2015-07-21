using System.Collections.Generic;

namespace ActivityStreams
{
    public class ActivityStream
    {
        readonly HashSet<ActivityStreamItem> items;

        public ActivityStream()
        {
            items = new HashSet<ActivityStreamItem>();
        }

        public void Append(ActivityStreamItem item)
        {

        }
    }
}