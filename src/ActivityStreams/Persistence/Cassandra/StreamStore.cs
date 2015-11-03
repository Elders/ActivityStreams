using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class StreamStore : IStreamStore
    {
        const string StoreFeedStreamQueryTemplate = @"INSERT INTO ""streams"" (sid,asid,ts) VALUES (?,?,?);";

        const string DeleteFeedStreamQueryTemplate = @"DELETE FROM ""feedstreams"" WHERE fid=? AND sid=?;";

        const string LoadFeedStreamQueryTemplate = @"SELECT * FROM ""streams"" where sid=?;";

        readonly ISession session;

        public StreamStore(ISession session)
        {
            this.session = session;
        }

        //public void Save(IStream feedStream)
        //{
        //    var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
        //    var fid = Convert.ToBase64String(feedStream.FeedId);
        //    var sid = feedStream.StreamId;
        //    session
        //        .Execute(prepared
        //        .Bind(fid, sid));
        //}

        //public void Detach(IStream source, IStream attachedStream, long endDate)
        //{
        //    //var prepared = session.Prepare(DeleteFeedStreamQueryTemplate);
        //    //var fid = Convert.ToBase64String(feedStream.FeedId);
        //    //var sid = feedStream.StreamId;
        //    //session
        //    //    .Execute(prepared
        //    //    .Bind(fid, sid));
        //}

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

        public void Attach(byte[] sourceStreamId, byte[] streamIdToDetach, long expiresAt)
        {
            var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
            var sid = Convert.ToBase64String(sourceStreamId);
            var asid = Convert.ToBase64String(streamIdToDetach);
            long ts = expiresAt == long.MaxValue ? 0 : expiresAt;
            session
                .Execute(prepared
                .Bind(sid, asid, ts));
        }

        public void Detach(byte[] sourceStreamId, byte[] streamIdToDetach, long detachedSince)
        {
            throw new NotImplementedException();
        }

        public ActivityStream Get(byte[] streamId)
        {
            var sid = Convert.ToBase64String(streamId);
            var prepared = session
                    .Prepare(LoadFeedStreamQueryTemplate)
                    .Bind(sid);

            var rowSet = session.Execute(prepared);
            var queryResult = rowSet.GetRows();

            var result = new ActivityStream(streamId);
            List<byte[]> feedStreams = new List<byte[]>();
            bool isLoaded = false;
            foreach (var row in queryResult)
            {
                isLoaded = true;
                var asid = Convert.FromBase64String(row.GetValue<string>("asid"));
                var ts = row.GetValue<long>("ts");
                long expiresAt = ts == 0 ? long.MaxValue : ts;
                result.Attach(asid, expiresAt);
            }

            return isLoaded ? result : null;
        }
    }
}
