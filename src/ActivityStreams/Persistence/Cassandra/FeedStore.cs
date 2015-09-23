using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    public class FeedRepo : IFeedRepository
    {
        readonly FeedStore store;
        public FeedRepo(FeedStore store)
        {
            this.store = store;
        }

        public Feed Get(byte[] id)
        {
            return store.Get(id);
        }

        public void Save(Feed feed)
        {
            store.Save(feed);
        }
    }

    public class FeedStore
    {
        const string StoreFeedQueryTemplate = @"INSERT INTO feeds (fid,fs) VALUES (?,?);";

        const string GetFeedQueryTemplate = @"SELECT data FROM ""activities_desc"" where sid=? AND ts<=?;";

        readonly ISerializer serializer;

        readonly ISession session;

        public FeedStore(ISession session, ISerializer serializer)
        {
            this.session = session;
            this.serializer = serializer;
        }

        public void Save(Feed feed)
        {
            var prepared = session.Prepare(StoreFeedQueryTemplate);

            session
                .Execute(prepared
                .Bind(Convert.ToBase64String(feed.Id), feed.FeedStreams));
        }

        public Feed Get(byte[] feedId)
        {
            var feedIdQueryable = Convert.ToBase64String(feedId);

            var prepared = session
                    .Prepare(GetFeedQueryTemplate)
                    .Bind(feedIdQueryable);

            var rowSet = session.Execute(prepared);
            var feedStreams = rowSet.GetRows().Single().GetValue<List<FeedStream>>("fsd");
            var feed = new Feed(new byte[1] { 1 });
            return feed;
        }

        byte[] SerializeActivity(Activity activity)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, activity);
                return stream.ToArray();
            }
        }
    }
}
