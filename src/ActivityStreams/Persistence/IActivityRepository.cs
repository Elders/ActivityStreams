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
    }
}
