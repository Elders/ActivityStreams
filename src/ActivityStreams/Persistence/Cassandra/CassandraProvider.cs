using System;
using Cassandra;
using Microsoft.Extensions.Configuration;

namespace ActivityStreams.Persistence.Cassandra
{
    public class CassandraProvider : ICassandraProvider
    {
        public const string ConnectionStringSettingKey = "activitystreams_cassandra_connectionstring";

        protected readonly IConfiguration configuration;
        protected readonly ICassandraReplicationStrategy replicationStrategy;
        protected readonly IInitializer initializer;

        protected Cluster cluster;
        protected ISession session;

        protected string baseConfigurationKeyspace;

        public CassandraProvider(IConfiguration configuration, ICassandraReplicationStrategy replicationStrategy, IInitializer initializer = null)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (replicationStrategy is null) throw new ArgumentNullException(nameof(replicationStrategy));

            this.configuration = configuration;
            this.replicationStrategy = replicationStrategy;
            this.initializer = initializer;
        }

        public Cluster GetCluster()
        {
            if (cluster is null == false)
                return cluster;

            Builder builder = initializer as Builder;
            if (builder is null)
            {
                builder = Cluster.Builder();
                //  TODO: check inside the `cfg` (var cfg = builder.GetConfiguration();) if we already have connectionString specified

                string connectionString = configuration[ConnectionStringSettingKey];

                var hackyBuilder = new CassandraConnectionStringBuilder(connectionString);
                if (string.IsNullOrEmpty(hackyBuilder.DefaultKeyspace) == false)
                    connectionString = connectionString.Replace(hackyBuilder.DefaultKeyspace, "");
                baseConfigurationKeyspace = hackyBuilder.DefaultKeyspace;

                var connStrBuilder = new CassandraConnectionStringBuilder(connectionString);
                cluster = connStrBuilder
                    .ApplyToBuilder(builder)
                    .Build();
            }

            else
            {
                cluster = Cluster.BuildFrom(initializer);
            }

            return cluster;
        }

        protected virtual string GetKeyspace()
        {
            var keyspace = baseConfigurationKeyspace;
            if (keyspace.Length > 48) throw new ArgumentException($"Cassandra keyspace exceeds maximum length of 48. Keyspace: {keyspace}");

            return keyspace;
        }

        public ISession GetSession()
        {
            if (session is null)
            {
                session = GetCluster().Connect();
                CreateKeyspace(GetKeyspace(), replicationStrategy);
            }

            return session;
        }

        private void CreateKeyspace(string keyspace, ICassandraReplicationStrategy replicationStrategy)
        {
            var createKeySpaceQuery = replicationStrategy.CreateKeySpaceTemplate(keyspace);
            session.Execute(createKeySpaceQuery);
            session.ChangeKeyspace(keyspace);
        }
    }
}
