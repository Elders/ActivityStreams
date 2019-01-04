using System;
using System.Threading.Tasks;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class StreamService
    {
        readonly IStreamRepository repository;

        public StreamService(IStreamRepository repository)
        {
            this.repository = repository;
        }

        public void Attach(byte[] streamId, byte[] streamIdToAttach)
        {
            var stream = repository.Load(streamId);
            var result = stream.Attach(streamIdToAttach);
            if (result.IsSuccessful)
                repository.AttachStream(streamId, streamIdToAttach, ActivityStream.DefaultExpirationTimestamp);
        }

        public void Attach(byte[] streamId, byte[] streamIdToAttach, DateTime expiresAt)
        {
            if (ByteArrayHelper.Compare(streamId, streamIdToAttach))
                throw new ArgumentException("Attaching a stream to itself is now allowed.");

            var stream = repository.Load(streamId);
            var result = stream.Attach(streamIdToAttach, expiresAt.ToFileTimeUtc());
            if (result.IsSuccessful)
                repository.AttachStream(streamId, streamIdToAttach, expiresAt.ToFileTimeUtc());
        }

        public void Detach(byte[] streamId, byte[] streamIdToDetach, DateTime detachedSince)
        {
            if (ByteArrayHelper.Compare(streamId, streamIdToDetach))
                throw new ArgumentException("Detaching a stream from itself is now allowed.");

            var stream = repository.Load(streamId);
            var result = stream.Detach(streamIdToDetach, detachedSince);
            if (result.IsSuccessful)
                repository.DetachStream(streamId, streamIdToDetach, detachedSince.ToFileTimeUtc());
        }

        public ActivityStream Get(byte[] streamId)
        {
            return repository.Load(streamId);
        }

        public Task<ActivityStream> GetAsync(byte[] streamId)
        {
            return repository.LoadAsync(streamId);
        }
    }
}
