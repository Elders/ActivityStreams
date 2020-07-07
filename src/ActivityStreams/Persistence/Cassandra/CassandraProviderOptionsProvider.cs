using Microsoft.Extensions.Configuration;

namespace ActivityStreams.Persistence.Cassandra
{
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
