using System;
using System.Collections.Generic;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class FeedStreamStore : IFeedStreamStore
    {
        const string StoreFeedStreamQueryTemplate = @"INSERT INTO ""feedstreams"" (fid,sid) VALUES (?,?);";

        const string DeleteFeedStreamQueryTemplate = @"DELETE FROM ""feedstreams"" WHERE fid=? AND sid=?;";

        const string LoadFeedStreamQueryTemplate = @"SELECT sid FROM ""feedstreams"" where fid=?;";

        readonly ISession session;

        public FeedStreamStore(ISession session)
        {
            this.session = session;
        }

        public void Save(IStream feedStream)
        {
            var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
            var fid = Convert.ToBase64String(feedStream.FeedId);
            var sid = feedStream.StreamId;
            session
                .Execute(prepared
                .Bind(fid, sid));
        }

        public void Delete(IStream feedStream)
        {
            var prepared = session.Prepare(DeleteFeedStreamQueryTemplate);
            var fid = Convert.ToBase64String(feedStream.FeedId);
            var sid = feedStream.StreamId;
            session
                .Execute(prepared
                .Bind(fid, sid));
        }

        public IEnumerable<IStream> Load(byte[] feedId)
        {
            var fid = Convert.ToBase64String(feedId);
            var prepared = session
                    .Prepare(LoadFeedStreamQueryTemplate)
                    .Bind(fid);

            var rowSet = session.Execute(prepared);
            IList<IStream> feedStreams = new List<IStream>();
            foreach (var row in rowSet.GetRows())
            {
                var stream = row.GetValue<byte[]>("sid");
                var feedStream = new Stream(feedId, stream);
                feedStreams.Add(feedStream);
            }

            return feedStreams;
        }
    }
}
