using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public interface ICassandraProvider
    {
        Cluster GetCluster();
        ISession GetSession();
    }
}
