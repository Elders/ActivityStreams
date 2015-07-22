using System.Collections.Generic;
using System.Linq;

namespace ActivityStreams
{
    public class Subscription
    {
        readonly HashSet<byte[]> streamSubscriptions;

        public Subscription(byte[] owner)
        {
            Owner = owner;
            streamSubscriptions = new HashSet<byte[]>();
        }

        public byte[] Owner { get; }

        public IEnumerable<byte[]> Streams { get { return streamSubscriptions.ToList().AsReadOnly(); } }

        public void SubscribeTo(byte[] streamId)
        {
            streamSubscriptions.Add(streamId);
        }
    }
}
