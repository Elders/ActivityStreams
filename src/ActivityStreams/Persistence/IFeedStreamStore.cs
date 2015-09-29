using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IFeedStreamStore
    {
        IEnumerable<IStream> Load(byte[] feedId);

        void Save(IStream feedStream);

        void Delete(IStream feedStream);
    }
}
