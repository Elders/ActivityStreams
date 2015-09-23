using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cassandra;
using Elders.Proteus;
using ActivityStreams.Persistence.InMemory;

namespace ActivityStreams.Persistence.Cassandra
{

    public class ActivityStreamsStorageManager
    {
        const string CreateKeySpaceTemplate = @"CREATE KEYSPACE IF NOT EXISTS ""activitystreams"" WITH replication = {{'class':'SimpleStrategy', 'replication_factor':1}};";
        const string CreateEventsTableTemplate = @"CREATE TABLE IF NOT EXISTS ""activities_desc"" (sid text, ts bigint, data blob, PRIMARY KEY (sid,ts)) WITH CLUSTERING ORDER BY (ts DESC);";

        readonly ISession session;

        public ActivityStreamsStorageManager(ISession session)
        {
            this.session = session;
        }

        public void CreateActivitiesStorage()
        {
            var createEventsTable = CreateEventsTableTemplate.ToLowerInvariant();
            session.Execute(createEventsTable);
        }
    }
}