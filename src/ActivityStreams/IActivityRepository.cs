using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityStreams.Helpers;
using ActivityStreams.Persistence;

namespace ActivityStreams
{
    public interface IActivityRepository
    {
        /// <summary>
        /// Appends an activity to a stream.
        /// </summary>
        void Append(Activity activity);

        /// <summary>
        /// Removes all activities from stream in specific point of time
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="timestamp">Utc time</param>
        void Remove(byte[] streamId, long timestamp);

        /// <summary>
        /// Loads all activities for the specified stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IEnumerable<Activity> Load(ActivityStream stream, ActivityStreamOptions options);

        Task<List<Activity>> LoadAsync(ActivityStream stream, ActivityStreamOptions options);
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

        public void Remove(byte[] streamId, long timestamp)
        {
            store.Delete(streamId, timestamp);
        }

        /// <summary>
        /// FanIn
        /// </summary>
        public IEnumerable<Activity> Load(ActivityStream stream, ActivityStreamOptions feedOptions)
        {
            // At this point we have all the streams with their timestamp
            Dictionary<byte[], long> streamsToLoad = new StreamCrawler(streamStore).StreamsToLoad(stream, feedOptions.Paging.Timestamp);

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

        public async Task<List<Activity>> LoadAsync(ActivityStream stream, ActivityStreamOptions options)
        {
            List<Activity> result = new List<Activity>();
            // At this point we have all the streams with their timestamp
            var crawler = new StreamCrawler(streamStore);
            Dictionary<byte[], long> streamsToLoad = await crawler.StreamsToLoadAsync(stream, options.Paging.Timestamp).ConfigureAwait(false);

            options = options ?? ActivityStreamOptions.Default;

            var snapshot = GetSnapshot(streamsToLoad, options);

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
                result.Add(nextActivity);

                FetchNextActivity(snapshot, nextActivity.StreamId, buffer);
            }

            return result;
        }
    }
}
