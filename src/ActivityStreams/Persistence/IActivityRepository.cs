using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence
{
    public interface IActivityRepository
    {
        /// <summary>
        /// Appends an activity to a stream.
        /// </summary>
        void Append(Activity activity);

        /// <summary>
        /// Loads all activities for the specified stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IEnumerable<Activity> Load(ActivityStream stream, ActivityStreamOptions options);
    }

    public class DefaultActivityRepository : IActivityRepository
    {
        readonly IActivityStore store;
        readonly IStreamStore streamStore;

        public DefaultActivityRepository(IActivityStore store, IStreamStore streamStore)
        {
            this.store = store;
            this.streamStore = streamStore;
        }

        public void Append(Activity activity)
        {
            store.Save(activity);
        }

        /// <summary>
        /// FanIn
        /// </summary>
        public IEnumerable<Activity> Load(ActivityStream stream, ActivityStreamOptions feedOptions)
        {
            // At this point we have all the streams with their timestamp
            Dictionary<byte[], long> streamsToLoad = new StreamCrawler(streamStore).StreamsToLoad(stream, feedOptions.Paging.Timestamp);
            Dictionary<string, DateTime> debug = streamsToLoad.ToDictionary(key => Encoding.UTF8.GetString(key.Key), val => DateTime.FromFileTimeUtc(val.Value));

            feedOptions = feedOptions ?? ActivityStreamOptions.Default;

            var snapshot = GetSnapshot(streamsToLoad, feedOptions);

            SortedSet<Activity> buffer = new SortedSet<Activity>(Activity.ComparerDesc);

            //  Init
            foreach (var str in streamsToLoad)
            {
                var streamId = str.Key;
                FetchNextActivity(snapshot, streamId, buffer);
            }

            while (buffer.Count > 0)
            {
                Activity nextActivity = buffer.First();
                buffer.Remove(nextActivity);
                yield return nextActivity;

                FetchNextActivity(snapshot, nextActivity.StreamId, buffer);
            }
        }

        /// <summary>
        /// Gets the first activity from the specified stream and places it into the specified buffer.
        /// </summary>
        void FetchNextActivity(Dictionary<byte[], Queue<Activity>> snapshot, byte[] streamId, SortedSet<Activity> buffer)
        {
            Queue<Activity> streamQueue;
            if (snapshot.TryGetValue(streamId, out streamQueue))
            {
                if (streamQueue.Count > 0)
                {
                    var candidate = streamQueue.Dequeue();
                    buffer.Add(candidate);
                }
            }
        }

        /// <summary>
        /// Gets unordered activities snapshot for the specified streams.
        /// </summary>
        Dictionary<byte[], Queue<Activity>> GetSnapshot(Dictionary<byte[], long> streams, ActivityStreamOptions options)
        {
            var snapshot = new Dictionary<byte[], Queue<Activity>>(new ByteArrayEqualityComparer());
            foreach (var stremToLoad in streams)
            {
                var timestamp = stremToLoad.Value < options.Paging.Timestamp ? stremToLoad.Value : options.Paging.Timestamp;
                var pagingOptions = new Paging(timestamp, options.Paging.Take);
                var newOptions = new ActivityStreamOptions(pagingOptions, options.SortOrder);

                snapshot.Add(stremToLoad.Key, new Queue<Activity>(store.LoadStream(stremToLoad.Key, newOptions)));
            }
            return snapshot;
        }
    }
}
