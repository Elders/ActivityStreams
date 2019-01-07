using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityStreams.Persistence
{
    public interface IActivityStore
    {
        void Save(Activity activity);
        void Delete(byte[] streamId, long timestamp);
        IEnumerable<Activity> LoadStream(byte[] streamId, ActivityStreamOptions options);
        Task<IEnumerable<Activity>> LoadStreamAsync(byte[] streamId, ActivityStreamOptions options);
    }
}
