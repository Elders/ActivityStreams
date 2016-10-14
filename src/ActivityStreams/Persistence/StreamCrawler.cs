using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams.Persistence
{
    /// <summary>
    /// Flattens the graph 'ActivityStreams' structure. If there are multiple nodes for a stream with different expiration
    /// timestamp then the lates expiration timestamp will be captured.
    /// </summary>
    public class StreamCrawler
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
            {
                if (crawledStreams[stream.StreamId] < stream.ExpiresAt)
                    crawledStreams[stream.StreamId] = stream.ExpiresAt;
            }
            else
                crawledStreams.Add(stream.StreamId, stream.ExpiresAt);
        }

        void Guard_AgainstStupidity()
        {
            stupidityCounter++;
            if (stupidityCounter > 10000)
                throw new InvalidOperationException("The stupidity counter reached its max value. How did you do that?");
        }

        public Dictionary<byte[], long> StreamsToLoad(ActivityStream stream, long timestamp = 0)
        {
            AddOrUpdateStreamsToLoadWith(streamsToLoad, stream, timestamp);
            Stack<byte[]> streamsToCrawl = new Stack<byte[]>();
            streamsToCrawl.Push(stream.StreamId);

            while (streamsToCrawl.Count > 0)
            {
                Guard_AgainstStupidity();

                // Load next stream.
                var currentId = streamsToCrawl.Pop();
                var current = streamStore.Get(currentId) ?? ActivityStream.Empty;
                long currentExpiresAt = current.ExpiresAt;
                streamsToLoad.TryGetValue(current.StreamId, out currentExpiresAt);

                if (ActivityStream.IsEmpty(current)) continue;

                // Crawl only first level attached streams.
                foreach (var nested in current.AttachedStreams)
                {
                    if (IsCrawled(nested)) continue;

                    streamsToCrawl.Push(nested.StreamId);

                    var nestedShouldExpireAt = currentExpiresAt < nested.ExpiresAt ? currentExpiresAt : nested.ExpiresAt;
                    AddOrUpdateStreamsToLoadWith(streamsToLoad, nested, nestedShouldExpireAt);
                }
            }

            return streamsToLoad;
        }

        void AddOrUpdateStreamsToLoadWith(Dictionary<byte[], long> streamsToLoad, ActivityStream stream, long timestamp = 0)
        {
            if (streamsToLoad.ContainsKey(stream.StreamId))
            {
                if (streamsToLoad[stream.StreamId] < timestamp)
                    streamsToLoad[stream.StreamId] = timestamp;
            }
            else
                streamsToLoad.Add(stream.StreamId, timestamp);

            MarkAsCrowled(stream);
        }
    }
}