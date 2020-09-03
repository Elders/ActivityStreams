using ActivityStreams.Persistence;
using ActivityStreams.Persistence.Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace ActivityStreams
{
    public static class ActivityStreamsServiceCollectionExtensions
    {
        public static IServiceCollection AddActivityStreams<TSerializer>(this IServiceCollection services, IConfiguration configuration)
            where TSerializer : class, ISerializer
        {
            AddActivityStreamOption<CassandraProviderOptions, CassandraProviderOptionsProvider>(services);

            services.AddSingleton<ISerializer, TSerializer>();
            services.AddSingleton<CassandraProvider, CassandraProvider>();
            services.AddSingleton<ICassandraProvider, CassandraProvider>();
            services.AddSingleton<IActivityStore, ActivityStore>();
            services.AddSingleton<IStreamStore, StreamStore>();
            services.AddSingleton<IActivityRepository, DefaultActivityRepository>();
            services.AddTransient<StreamService>();
            services.AddTransient<IStreamRepository, DefaultStreamRepository>();
            services.AddSingleton<StorageManager>();
            services.AddSingleton<CassandraReplicationStrategyFactory>();
            services.AddTransient(typeof(ICassandraReplicationStrategy), provider => provider.GetRequiredService<CassandraReplicationStrategyFactory>().GetReplicationStrategy());

            return services;
        }

        static IServiceCollection AddActivityStreamOption<T, V>(this IServiceCollection services)
            where T : class, new()
            where V : OptionsProviderBase<T>
        {
            services.AddSingleton<IConfigureOptions<T>, V>();
            services.AddSingleton<IOptionsChangeTokenSource<T>, V>();
            services.AddSingleton<IOptionsFactory<T>, V>();

            return services;
        }
    }

    class CassandraReplicationStrategyFactory
    {
        private readonly CassandraProviderOptions options;

        public CassandraReplicationStrategyFactory(IOptionsMonitor<CassandraProviderOptions> optionsMonitor)
        {
            this.options = optionsMonitor.CurrentValue;
        }

        internal ICassandraReplicationStrategy GetReplicationStrategy()
        {
            ICassandraReplicationStrategy replicationStrategy = null;
            if (options.ReplicationStrategy.Equals("simple", StringComparison.OrdinalIgnoreCase))
            {
                replicationStrategy = new SimpleReplicationStrategy(options.ReplicationFactor);
            }
            else if (options.ReplicationStrategy.Equals("network_topology", StringComparison.OrdinalIgnoreCase))
            {
                var settings = new List<NetworkTopologyReplicationStrategy.DataCenterSettings>();
                foreach (var datacenter in options.Datacenters)
                {
                    var setting = new NetworkTopologyReplicationStrategy.DataCenterSettings(datacenter, options.ReplicationFactor);
                    settings.Add(setting);
                }
                replicationStrategy = new NetworkTopologyReplicationStrategy(settings);
            }

            return replicationStrategy;
        }
    }
}
