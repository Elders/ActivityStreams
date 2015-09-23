using System;
using System.Linq;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra.Tests.Helpers
{
    public static class SessionExtensionsForIntegrationTests
    {
        public static byte[] GetBytesById(this ISession session, byte[] id)
        {
            var row = session
                .Execute($"select * from activities_desc where sid='{Convert.ToBase64String(id)}'")
                .GetRows()
                .First();

            return row.GetValue<byte[]>("data");
        }
    }
}
