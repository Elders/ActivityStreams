using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public interface ICassandraProvider
    {
        ICluster GetCluster();
        ISession GetSession();
    }
}
