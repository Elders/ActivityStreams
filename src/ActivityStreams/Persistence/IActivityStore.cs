using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        void Save(Activity activity);
        IEnumerable<Activity> LoadStream(byte[] streamId, Paging paging);
    }
}
