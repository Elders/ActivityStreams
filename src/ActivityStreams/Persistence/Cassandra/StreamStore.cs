using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class StreamStore : IStreamStore
    {
        const string StoreFeedStreamQueryTemplate = @"INSERT INTO ""streams"" (sid,asid,ts) VALUES (?,?,?);";

        const string DeleteFeedStreamQueryTemplate = @"DELETE FROM ""feedstreams"" WHERE fid=? AND sid=?;";

        const string LoadFeedStreamQueryTemplate = @"SELECT * FROM ""streams"" where sid=?;";

        private ISession GetSession() => cassandraProvider.GetSession();

        private readonly ICassandraProvider cassandraProvider;

        public StreamStore(ICassandraProvider cassandraProvider)
        {
            this.cassandraProvider = cassandraProvider ?? throw new ArgumentNullException(nameof(cassandraProvider)); ;
        }

        public void Attach(byte[] sourceStreamId, byte[] streamIdToAttach, long expiresAt)
        {
            var prepared = GetSession().Prepare(StoreFeedStreamQueryTemplate);
            var sid = Convert.ToBase64String(sourceStreamId);
            var asid = Convert.ToBase64String(streamIdToAttach);
            long ts = expiresAt == ActivityStream.DefaultExpirationTimestamp ? 0 : expiresAt;
            GetSession()
                .Execute(prepared
                .Bind(sid, asid, ts));
        }

        public void Detach(byte[] sourceStreamId, byte[] streamIdToDetach, long detachedSince)
        {
            var prepared = GetSession().Prepare(StoreFeedStreamQueryTemplate);
            var sid = Convert.ToBase64String(sourceStreamId);
            var asid = Convert.ToBase64String(streamIdToDetach);
            long ts = detachedSince == ActivityStream.DefaultExpirationTimestamp ? 0 : detachedSince;
            GetSession()
                .Execute(prepared
                .Bind(sid, asid, ts));
        }

        public ActivityStream Get(byte[] streamId)
        {
            var sid = Convert.ToBase64String(streamId);
            var query = GetTemplate().Bind(sid);

            var rowSet = GetSession().Execute(query);
            var result = new ActivityStream(streamId);
            List<byte[]> feedStreams = new List<byte[]>();
            bool isLoaded = false;
            foreach (var row in rowSet.GetRows())
            {
                isLoaded = true;
                var asid = Convert.FromBase64String(row.GetValue<string>("asid"));
                var ts = row.GetValue<long>("ts");
                long expiresAt = ts == 0 ? ActivityStream.DefaultExpirationTimestamp : ts;
                result.Attach(asid, expiresAt);
            }

            return isLoaded ? result : null;
        }

        PreparedStatement prepared_GET;

        private async Task<PreparedStatement> GetTemplateAsync()
        {
            if (prepared_GET is null)
                prepared_GET = await GetSession().PrepareAsync(LoadFeedStreamQueryTemplate).ConfigureAwait(false);

            return prepared_GET;
        }

        private PreparedStatement GetTemplate()
        {
            if (prepared_GET is null)
                prepared_GET = GetSession().Prepare(LoadFeedStreamQueryTemplate);

            return prepared_GET;
        }

        public async Task<ActivityStream> GetAsync(byte[] streamId)
        {
            var sid = Convert.ToBase64String(streamId);
            var statement = await GetTemplateAsync().ConfigureAwait(false);
            var query = statement.Bind(sid);

            var rowSet = await GetSession().ExecuteAsync(query).ConfigureAwait(false);
            var result = new ActivityStream(streamId);
            List<byte[]> feedStreams = new List<byte[]>();
            bool isLoaded = false;
            foreach (var row in rowSet.GetRows())
            {
                isLoaded = true;
                var asid = Convert.FromBase64String(row.GetValue<string>("asid"));
                var ts = row.GetValue<long>("ts");
                long expiresAt = ts == 0 ? ActivityStream.DefaultExpirationTimestamp : ts;
                result.Attach(asid, expiresAt);
            }

            return isLoaded ? result : null;
        }
    }
}
