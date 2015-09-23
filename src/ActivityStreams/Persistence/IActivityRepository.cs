using System;
using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityRepository
    {
        /// <summary>
        /// Appends an activity to a stream.
        /// </summary>
        void Append(Activity activity);

        /// <summary>
        /// Loads all activities associated with the streams for the specified feed.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        IEnumerable<Activity> Load(Feed feed);

        /// <summary>
        /// Loads all activities associated with the streams for the specified feed.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        IEnumerable<Activity> Load(Feed feed, DateTime timestamp);
    }

    public class ActivityRepository : IActivityRepository
    {
        IActivityStore store;

        public ActivityRepository(IActivityStore store)
        {
            this.store = store;
        }

        public void Append(Activity activity)
        {
            store.Save(activity);
        }

        public IEnumerable<Activity> Load(Feed feed)
        {
            return Load(feed, DateTime.UtcNow);
        }

        public IEnumerable<Activity> Load(Feed feed, DateTime timestamp)
        {
            var result = store.Get(feed, new Paging(timestamp.ToFileTimeUtc(), 20));
            return result;
        }
    }
}
