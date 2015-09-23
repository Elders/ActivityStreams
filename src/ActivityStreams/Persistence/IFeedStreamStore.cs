using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IFeedStreamStore
    {
        IEnumerable<FeedStream> Load(byte[] feedId);

        void Save(FeedStream feedStream);

        void Delete(FeedStream feedStream);
    }
}
