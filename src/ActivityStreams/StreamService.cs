using System;
using ActivityStreams.Persistence;

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
                repository.AttachStream(streamId, streamIdToAttach);
        }

        public void Attach(byte[] streamId, byte[] streamIdToAttach, DateTime expiresAt)
        {
            var stream = repository.Load(streamId);
            var result = stream.Attach(streamIdToAttach, expiresAt.ToFileTimeUtc());
            if (result.IsSuccessful)
                repository.AttachStream(streamId, streamIdToAttach);
        }

        public void Detach(byte[] streamId, byte[] streamIdToDetach, DateTime detachedSince)
        {
            var stream = repository.Load(streamId);
            var result = stream.Detach(streamIdToDetach, detachedSince);
            if (result.IsSuccessful)
                repository.DetachStream(streamId, streamIdToDetach, detachedSince.ToFileTimeUtc());
        }

        public ActivityStream Get(byte[] streamId)
        {
            return repository.Load(streamId);
        }
    }
}
