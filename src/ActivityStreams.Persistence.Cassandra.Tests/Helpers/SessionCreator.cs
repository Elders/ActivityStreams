using Cassandra;

namespace ActivityStreams.Persistence.Cassandra.Tests.Helpers
{
    public class SessionCreator
    {
        public static ISession Create(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = System.Configuration.ConfigurationManager.AppSettings["actstr-testc-int"];

            var cluster = Cluster.Builder()
                .WithConnectionString(connectionString)
                .Build();
            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
            return session;
        }
    }
}
