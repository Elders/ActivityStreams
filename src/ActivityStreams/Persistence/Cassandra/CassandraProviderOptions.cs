using System;
using System.Collections.Generic;

namespace ActivityStreams.Persistence.Cassandra
{
    public class CassandraProviderOptions : IEquatable<CassandraProviderOptions>
    {
        public string ConnectionString { get; set; }

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
}
