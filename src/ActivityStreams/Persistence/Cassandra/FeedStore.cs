using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void Save(FeedStream feedStream)
        {
            var prepared = session.Prepare(StoreFeedStreamQueryTemplate);
            var fid = Convert.ToBase64String(feedStream.FeedId);
            var sid = Convert.ToBase64String(feedStream.StreamId);
            session
                .Execute(prepared
                .Bind(fid, sid));
        }

        public void Delete(FeedStream feedStream)
        {
            var prepared = session.Prepare(DeleteFeedStreamQueryTemplate);
            var fid = Convert.ToBase64String(feedStream.FeedId);
            var sid = Convert.ToBase64String(feedStream.StreamId);
            session
                .Execute(prepared
                .Bind(fid, sid));
        }

        public IEnumerable<FeedStream> Load(byte[] feedId)
        {
            var fid = Convert.ToBase64String(feedId);
            var prepared = session
                    .Prepare(LoadFeedStreamQueryTemplate)
                    .Bind(fid);

            var rowSet = session.Execute(prepared);
            IList<FeedStream> feedStreams = new List<FeedStream>();
            foreach (var row in rowSet.GetRows())
            {
                var stream = row.GetValue<byte[]>("sid");
                var feedStream = new FeedStream(feedId, stream);
                feedStreams.Add(feedStream);
            }

            return feedStreams;
        }
    }
}
