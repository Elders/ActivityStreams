using System;

namespace ActivityStreams.Persistence.Cassandra
{
    public class SimpleReplicationStrategy : ICassandraReplicationStrategy
    {
        private const string keySpaceTemplate = @"CREATE KEYSPACE IF NOT EXISTS {0} WITH replication = {{'class':'SimpleStrategy', 'replication_factor':{1}}};";

        public SimpleReplicationStrategy(int replicationFactor)
        {
            if (replicationFactor < 1) throw new ArgumentException("Replication factor should be at least '1'. Default is 1", nameof(replicationFactor));

            ReplicationFactor = replicationFactor;
        }

        public int ReplicationFactor { get; private set; }

        public string CreateKeySpaceTemplate(string keySpace)
        {
            if (string.IsNullOrEmpty(keySpace)) throw new ArgumentNullException(nameof(keySpace));

            return string.Format(keySpaceTemplate, keySpace, ReplicationFactor);
        }
    }
}
