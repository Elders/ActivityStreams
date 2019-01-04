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
            services.AddTransient<IActivityStore, ActivityStore>(provider => new ActivityStore(provider.GetService<ICassandraProvider>().GetSession(), provider.GetService<ISerializer>()));
            services.AddTransient<IStreamStore, StreamStore>(provider => new StreamStore(provider.GetService<ICassandraProvider>()));
            services.AddSingleton<IActivityRepository, DefaultActivityRepository>();
            services.AddTransient<StreamService>();
            services.AddTransient<IStreamRepository, DefaultStreamRepository>();
            services.AddTransient(typeof(ICassandraReplicationStrategy), provider => GetReplicationStrategy(configuration));

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

        static int GetReplicationFactor(IConfiguration configuration)
        {
            var replFactorCfg = configuration["activitystreams_persistence_cassandra_replication_factor"];
            return string.IsNullOrEmpty(replFactorCfg) ? 1 : int.Parse(replFactorCfg);
        }

        static ICassandraReplicationStrategy GetReplicationStrategy(IConfiguration configuration)
        {
            var replStratefyCfg = configuration["activitystreams_persistence_cassandra_replication_strategy"];
            var replFactorCfg = configuration["activitystreams_persistence_cassandra_replication_factor"];

            ICassandraReplicationStrategy replicationStrategy = null;
            if (string.IsNullOrEmpty(replStratefyCfg))
            {
                replicationStrategy = new SimpleReplicationStrategy(1);
            }
            else if (replStratefyCfg.Equals("simple", StringComparison.OrdinalIgnoreCase))
            {
                replicationStrategy = new SimpleReplicationStrategy(GetReplicationFactor(configuration));
            }
            else if (replStratefyCfg.Equals("network_topology", StringComparison.OrdinalIgnoreCase))
            {
                int replicationFactor = GetReplicationFactor(configuration);
                var settings = new List<NetworkTopologyReplicationStrategy.DataCenterSettings>();
                string[] datacenters = configuration["activitystreams_persistence_cassandra__datacenters"].Split(',');
                foreach (var datacenter in datacenters)
                {
                    var setting = new NetworkTopologyReplicationStrategy.DataCenterSettings(datacenter, replicationFactor);
                    settings.Add(setting);
                }
                replicationStrategy = new NetworkTopologyReplicationStrategy(settings);
            }

            return replicationStrategy;
        }
    }
}
