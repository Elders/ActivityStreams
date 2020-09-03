using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ActivityStreams.Persistence.Cassandra
{
    public class CassandraProviderOptions : IEquatable<CassandraProviderOptions>
    {
        public CassandraProviderOptions()
        {
            Datacenters = new List<string>();
        }

        public string ConnectionString { get; set; }

        public string ReplicationStrategy { get; set; } = "simple";

        public int ReplicationFactor { get; set; } = 1;

        public List<string> Datacenters { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CassandraProviderOptions);
        }

        public bool Equals(CassandraProviderOptions other)
        {
            return other != null &&
                   ConnectionString == other.ConnectionString;
        }

        public override int GetHashCode()
        {
            return -887524364 + EqualityComparer<string>.Default.GetHashCode(ConnectionString);
        }

        public static bool operator ==(CassandraProviderOptions left, CassandraProviderOptions right)
        {
            return EqualityComparer<CassandraProviderOptions>.Default.Equals(left, right);
        }

        public static bool operator !=(CassandraProviderOptions left, CassandraProviderOptions right)
        {
            return !(left == right);
        }
    }

    public class CassandraProviderOptionsProvider : OptionsProviderBase<CassandraProviderOptions>
    {
        public const string SettingKey = "activitystreams:cassandra";

        public CassandraProviderOptionsProvider(IConfiguration configuration) : base(configuration) { }

        public override void Configure(CassandraProviderOptions options)
        {
            configuration.GetSection(SettingKey).Bind(options);
        }
    }
}
