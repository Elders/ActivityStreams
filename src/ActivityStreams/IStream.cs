using System;
using System.Collections.Generic;

namespace ActivityStreams
{
    public interface IStream
    {
        byte[] FeedId { get; set; }

        byte[] StreamId { get; set; }

        IEnumerable<IStream> Streams { get; }
    }

    public interface IFeed : IStream
    {
        void Attach(IStream stream);
        void Detach(IStream stream);
        void Close(DateTime endDate);
    }
}