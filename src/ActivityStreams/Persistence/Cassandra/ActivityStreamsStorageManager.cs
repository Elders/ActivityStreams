using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class ActivityStreamsStorageManager
    {
        const string CreateKeySpaceTemplate = @"CREATE KEYSPACE IF NOT EXISTS ""activitystreams"" WITH replication = {{'class':'SimpleStrategy', 'replication_factor':1}};";

        const string CreateEventsTableTemplate = @"CREATE TABLE IF NOT EXISTS ""activities_desc"" (sid text, ts bigint, data blob, PRIMARY KEY (sid,ts)) WITH CLUSTERING ORDER BY (ts DESC);";

        const string CreateFeedsTableTemplate = @"CREATE TABLE IF NOT EXISTS ""feedstreams"" (fid text, sid blob, PRIMARY KEY (fid,sid));";

        readonly ISession session;

        public ActivityStreamsStorageManager(ISession session)
        {
            this.session = session;
        }

        public void CreateActivitiesStorage()
        {
            var tableQ = CreateEventsTableTemplate.ToLowerInvariant();
            session.Execute(tableQ);
        }

        public void CreateFeedsStorage()
        {
            var tableQ = CreateFeedsTableTemplate.ToLowerInvariant();
            session.Execute(tableQ);
        }
    }
}
