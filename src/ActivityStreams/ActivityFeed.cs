using System;
using System.Collections.Generic;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public class ActivityFeed
    {
        readonly HashSet<Subscription> subscriptions;

        public ActivityFeed(byte[] ownerId)
        {
            OwnerId = ownerId;
            subscriptions = new HashSet<Subscription>();
        }

        public byte[] OwnerId { get; }

        public IEnumerable<byte[]> Streams { get { return subscriptions.Select(x => x.StreamId).ToList(); } }

        public void AddStream(byte[] streamId)
        {
            var subscription = new Subscription(OwnerId, streamId);
            subscriptions.Add(subscription);
        }
    }
}