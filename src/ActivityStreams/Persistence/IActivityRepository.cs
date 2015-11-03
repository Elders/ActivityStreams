using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Activity> Load(ActivityStream feed, ActivityStreamOptions feedOptions)
        {
            // At this point we have all the streams with their timestamp
            Dictionary<byte[], long> streamsToLoad = new StreamCrawler(streamStore).StreamsToLoad(feed.StreamId);

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
                var pagingOptions = new Paging(stremToLoad.Value, options.Paging.Take);
                snapshot.Add(stremToLoad.Key, new Queue<Activity>(store.LoadStream(stremToLoad.Key, pagingOptions)));
            }
            return snapshot;
        }

        /// <summary>
        /// Flattens the graph 'ActivityStreams' structure. If there are multiple nodes for a stream with different expiration
        /// timestamp then the lates expiration timestamp will be captured.
        /// </summary>
        class StreamCrawler
        {
            readonly IStreamStore streamStore;

            Dictionary<byte[], long> crawledStreams;
            Dictionary<byte[], long> streamsToLoad;
            int stupidityCounter;

            public StreamCrawler(IStreamStore streamStore)
            {
                this.streamStore = streamStore;
                crawledStreams = new Dictionary<byte[], long>(ByteArrayEqualityComparer.Default);
                streamsToLoad = new Dictionary<byte[], long>(ByteArrayEqualityComparer.Default);
                stupidityCounter = 0;
            }

            bool IsCrawled(ActivityStream stream)
            {
                return crawledStreams.ContainsKey(stream.StreamId) && crawledStreams[stream.StreamId] >= stream.ExpiresAt;
            }

            void MarkAsCrowled(ActivityStream stream)
            {
                if (crawledStreams.ContainsKey(stream.StreamId))
                    crawledStreams[stream.StreamId] = stream.ExpiresAt;
                else
                    crawledStreams.Add(stream.StreamId, stream.ExpiresAt);
            }

            void Guard_AgainstStupidity()
            {
                stupidityCounter++;
                if (stupidityCounter > 10000)
                    throw new InvalidOperationException("The stupidity counter reached its max value. How did you do that?");
            }

            public Dictionary<byte[], long> StreamsToLoad(byte[] feedId)
            {
                Stack<byte[]> streamsToCrawl = new Stack<byte[]>();
                streamsToCrawl.Push(feedId);

                while (streamsToCrawl.Count > 0)
                {
                    Guard_AgainstStupidity();

                    // Load next stream.
                    var currentId = streamsToCrawl.Pop();
                    var current = streamStore.Get(currentId) ?? ActivityStream.Empty;

                    if (ActivityStream.IsEmpty(current)) continue;

                    // Crawl only first level attached streams.
                    foreach (var nested in current.AttachedStreams)
                    {
                        if (IsCrawled(nested)) continue;

                        streamsToCrawl.Push(nested.StreamId);

                        AddOrUpdateStreamsToLoadWith(streamsToLoad, nested);
                    }
                    AddOrUpdateStreamsToLoadWith(streamsToLoad, current);
                }

                return streamsToLoad;
            }

            void AddOrUpdateStreamsToLoadWith(Dictionary<byte[], long> streamsToLoad, ActivityStream stream)
            {
                if (streamsToLoad.ContainsKey(stream.StreamId))
                {
                    if (streamsToLoad[stream.StreamId] < stream.ExpiresAt)
                        streamsToLoad[stream.StreamId] = stream.ExpiresAt;
                }
                else
                    streamsToLoad.Add(stream.StreamId, stream.ExpiresAt);

                MarkAsCrowled(stream);
            }
        }
    }
}
