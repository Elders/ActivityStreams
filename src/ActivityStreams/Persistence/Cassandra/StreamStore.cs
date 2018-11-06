using System;
using System.Collections.Generic;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class StreamStore : IStreamStore
    {
        const string StoreFeedStreamQueryTemplate = @"INSERT INTO ""streams"" (sid,asid,ts) VALUES (?,?,?);";

        const string DeleteFeedStreamQueryTemplate = @"DELETE FROM ""feedstreams"" WHERE fid=? AND sid=?;";

        const string LoadFeedStreamQueryTemplate = @"SELECT * FROM ""streams"" where sid=?;";

        private readonly ISession session;

        public StreamStore(ICassandraProvider cassandraProvider)
        {
            if (cassandraProvider is null) throw new ArgumentNullException(nameof(cassandraProvider));

            this.session = cassandraProvider.GetSession();
        }

        public IEnumerable<byte[]> Load(byte[] feedId)
        {
            var fid = Convert.ToBase64String(feedId);
            var prepared = session
                    .Prepare(LoadFeedStreamQueryTemplate)
                    .Bind(fid);

            var rowSet = session.Execute(prepared);
            List<byte[]> feedStreams = new List<byte[]>();
            foreach (var row in rowSet.GetRows())
            {
                var stream = row.GetValue<byte[]>("sid");
                feedStreams.Add(feedId);
            }

            return feedStreams;
        }

        public void Attach(byte[] sourceStreamId, byte[] streamIdToAttach, long expiresAt)
        {
            var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
            var sid = Convert.ToBase64String(sourceStreamId);
            var asid = Convert.ToBase64String(streamIdToAttach);
            long ts = expiresAt == ActivityStream.DefaultExpirationTimestamp ? 0 : expiresAt;
            session
                .Execute(prepared
                .Bind(sid, asid, ts));
        }

        public void Detach(byte[] sourceStreamId, byte[] streamIdToDetach, long detachedSince)
        {
            var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
            var sid = Convert.ToBase64String(sourceStreamId);
            var asid = Convert.ToBase64String(streamIdToDetach);
            long ts = detachedSince == ActivityStream.DefaultExpirationTimestamp ? 0 : detachedSince;
            session
                .Execute(prepared
                .Bind(sid, asid, ts));
        }

        public ActivityStream Get(byte[] streamId)
        {
            var sid = Convert.ToBase64String(streamId);
            var prepared = session
                    .Prepare(LoadFeedStreamQueryTemplate)
                    .Bind(sid);

            var rowSet = session.Execute(prepared);
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
