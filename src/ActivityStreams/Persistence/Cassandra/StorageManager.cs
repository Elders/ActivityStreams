using System;
using System.Threading;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class StorageManager
    {
        static int IsStorageCreationExecuted = 0;

        const string CreateEventsTableTemplateDesc = @"CREATE TABLE IF NOT EXISTS ""activities_desc"" (sid text, ts bigint, data blob, PRIMARY KEY (sid,ts)) WITH CLUSTERING ORDER BY (ts DESC);";

        const string CreateEventsTableTemplateAsc = @"CREATE TABLE IF NOT EXISTS ""activities_asc"" (sid text, ts bigint, data blob, PRIMARY KEY (sid,ts)) WITH CLUSTERING ORDER BY (ts ASC);";

        const string CreateFeedsTableTemplate = @"CREATE TABLE IF NOT EXISTS ""streams"" (sid text, asid text, ts bigint, PRIMARY KEY (sid,asid));";

        private ISession GetSession() => cassandraProvider.GetSession();

        private readonly ICassandraProvider cassandraProvider;

        public StorageManager(ICassandraProvider cassandraProvider)
        {
            this.cassandraProvider = cassandraProvider ?? throw new ArgumentNullException(nameof(cassandraProvider));
        }

        public void CreateStorage()
        {
            if (Interlocked.CompareExchange(ref IsStorageCreationExecuted, 1, 0) == 0)
            {
                var tableDesc = CreateEventsTableTemplateDesc.ToLower();
                GetSession().Execute(tableDesc);

                var tableAsc = CreateEventsTableTemplateAsc.ToLower();
                GetSession().Execute(tableAsc);

                var tableQ = CreateFeedsTableTemplate.ToLower();
                GetSession().Execute(tableQ);
            }
        }
    }
}
