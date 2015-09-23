using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        IEnumerable<Activity> Get(Feed feed, Paging paging);

        void Save(Activity activity);
    }
}
