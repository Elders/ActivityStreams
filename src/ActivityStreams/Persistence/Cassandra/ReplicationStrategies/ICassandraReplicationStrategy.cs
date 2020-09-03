namespace ActivityStreams.Persistence.Cassandra
{
    public interface ICassandraReplicationStrategy
    {
        string CreateKeySpaceTemplate(string keySpace);
    }
}
