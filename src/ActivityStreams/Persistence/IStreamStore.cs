using System.Threading.Tasks;

namespace ActivityStreams.Persistence
{
    public interface IStreamStore
    {
        ActivityStream Get(byte[] streamId);
        Task<ActivityStream> GetAsync(byte[] streamId);

        void Attach(byte[] sourceStreamId, byte[] streamIdToAttach, long expiresAt);

        void Detach(byte[] sourceStreamId, byte[] streamIdToDetach, long detachedSince);
    }
}
