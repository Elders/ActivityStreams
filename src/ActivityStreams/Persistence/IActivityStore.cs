using System;
using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        void Save(Activity activity);
        void Delete(byte[] streamId, long timestamp);
        IEnumerable<Activity> LoadStream(byte[] streamId, ActivityStreamOptions options);
    }
}
