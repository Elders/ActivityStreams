using System;
using System.Collections.Generic;

namespace ActivityStreams.Persistence.Cassandra
{
    public class NetworkTopologyReplicationStrategy : ICassandraReplicationStrategy
    {
        private const string keySpaceTemplate = @"CREATE KEYSPACE IF NOT EXISTS {0} WITH replication = {{'class':'NetworkTopologyStrategy'{1}}};";

        public NetworkTopologyReplicationStrategy(List<DataCenterSettings> settings)
        {
            if (ReferenceEquals(null, settings)) throw new ArgumentNullException(nameof(settings));
            if (settings.Count == 0) throw new ArgumentException("At least one datacenter configuration is required!", nameof(settings));

            Settings = settings;
        }

        public List<DataCenterSettings> Settings { get; private set; }

        public string CreateKeySpaceTemplate(string keySpace)
        {
            if (string.IsNullOrEmpty(keySpace)) throw new ArgumentNullException(nameof(keySpace));

            var dataCenterSettings = string.Empty;

            foreach (var dataCenter in Settings)
            {
                dataCenterSettings += $", '{dataCenter.Name}':{dataCenter.ReplicationFactor}";
            }

            return string.Format(keySpaceTemplate, keySpace, dataCenterSettings);
        }

        public class DataCenterSettings
        {
            public DataCenterSettings(string name, int replicationFactor)
            {
                Name = name;
                ReplicationFactor = replicationFactor;
            }

            public string Name { get; private set; }

            public int ReplicationFactor { get; private set; }
        }
    }
}
