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
        IEnumerable<Activity> Load(Feed feed, Paging paging);
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
            var paging = new Paging(DateTime.UtcNow.ToFileTimeUtc(), 20);
            return Load(feed, paging);
        }

        public IEnumerable<Activity> Load(Feed feed, Paging paging)
        {
            var result = store.Get(feed, paging);
            return result;
        }
    }
}
