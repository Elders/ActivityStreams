using System;
using System.Collections.Generic;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    public interface IStream
    {
        byte[] FeedId { get; set; }

        byte[] StreamId { get; set; }

        void Attach(IStream stream);
        void Detach(IStream stream);

        IEnumerable<IStream> Streams { get; }
    }
}