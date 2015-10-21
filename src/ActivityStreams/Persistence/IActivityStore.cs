using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        IEnumerable<Activity> Get(Feed feed, FeedOptions feedOptions);

        void Save(Activity activity);
    }
}
