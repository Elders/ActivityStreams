using System.Collections.Generic;

namespace ActivityStreams.Persistence
{
    public interface IActivityStreamRepository
    {
        void Append(Activity activity);

        IEnumerable<Activity> Load(Subscription subscription);
    }
}
