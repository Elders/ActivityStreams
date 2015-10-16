using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        IEnumerable<Activity> Get(Feed feed, Paging paging, SortOrder sortOrder);

        void Save(Activity activity);
    }
}
