using Microsoft.Extensions.Configuration;

namespace ActivityStreams.Persistence.Cassandra
{
    public class CassandraProviderOptionsProvider : OptionsProviderBase<CassandraProviderOptions>
    {
        public CassandraProviderOptionsProvider(IConfiguration configuration) : base(configuration) { }

        public override void Configure(CassandraProviderOptions options)
        {
            options.ConnectionString = configuration["activitystreams_cassandra_connectionstring"];
        }
    }
}
