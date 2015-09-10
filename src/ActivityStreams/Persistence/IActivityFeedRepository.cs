using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityFeedRepository
    {
        void Append(Activity activity);

        IEnumerable<Activity> Load(ActivityFeed feed);
    }
}
