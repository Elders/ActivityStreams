using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class Subscription
    {
        readonly HashSet<byte[]> streamSubscriptions;

        public Subscription(byte[] id, byte[] ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            streamSubscriptions = new HashSet<byte[]>(new ByteArrayEqualityComparer());
        }

        public byte[] Id { get; }

        public byte[] OwnerId { get; }

        public IEnumerable<byte[]> Streams { get { return streamSubscriptions.ToList().AsReadOnly(); } }

        public void SubscribeTo(byte[] streamId)
        {
            streamSubscriptions.Add(streamId);
        }
    }
}
