using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ActivityStreams.Persistence.InMemory;
using Cassandra;
using Elders.Proteus;

namespace ActivityStreams.Persistence.Cassandra
{
    public class ActivityRepository : IActivityRepository
    {
        ActivityStore store;

        public ActivityRepository(ActivityStore store)
        {
            this.store = store;
        }

        public void Append(Activity activity)
        {
            store.Save(activity);
        }

        public IEnumerable<Activity> Load(Feed feed)
        {
            return Load(feed, DateTime.UtcNow);
        }

        public IEnumerable<Activity> Load(Feed feed, DateTime timestamp)
        {
            var result = store.Get(feed, new Paging(timestamp.ToFileTimeUtc(), 20));
            return result;
        }
    }

    public class ActivityStore
    {
        const string AppendActivityStreamQueryTemplate = @"INSERT INTO activities_desc (sid,ts,data) VALUES (?,?,?);";

        const string LoadActivityStreamQueryTemplate = @"SELECT data FROM ""activities_desc"" where sid=? AND ts<=?;";

        readonly ISerializer serializer;

        readonly ISession session;

        public ActivityStore(ISession session, ISerializer serializer)
        {
            this.session = session;
            this.serializer = serializer;
        }

        public void Save(Activity activity)
        {
            var prepared = session.Prepare(AppendActivityStreamQueryTemplate);

            byte[] data = SerializeActivity(activity);
            session
                .Execute(prepared
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));
        }

        public IEnumerable<Activity> Get(Feed feed, Paging paging)
        {
            SortedSet<Activity> activities = new SortedSet<Activity>(Activity.Comparer);

            foreach (var streamId in feed.FeedStreams)
            {
                var streamIdQuery = Convert.ToBase64String(streamId);

                var prepared = session
                        .Prepare(LoadActivityStreamQueryTemplate)
                        .Bind(streamIdQuery, paging.Timestamp)
                        .SetAutoPage(false)
                        .SetPageSize(paging.Take);

                var rowSet = session.Execute(prepared);
                foreach (var row in rowSet.GetRows())
                {
                    using (var stream = new MemoryStream(row.GetValue<byte[]>("data")))
                    {
                        var storedActivity = (Activity)serializer.Deserialize(stream);
                        activities.Add(storedActivity);
                    }
                }
            }
            return activities.Take(paging.Take);
        }

        byte[] SerializeActivity(Activity activity)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, activity);
                return stream.ToArray();
            }
        }

        public class LoadSession
        { }
    }

    public class ActivityStreamsStorageManager
    {
        const string CreateKeySpaceTemplate = @"CREATE KEYSPACE IF NOT EXISTS ""activitystreams"" WITH replication = {{'class':'SimpleStrategy', 'replication_factor':1}};";
        const string CreateEventsTableTemplate = @"CREATE TABLE IF NOT EXISTS ""activities_desc"" (sid text, ts bigint, data blob, PRIMARY KEY (sid,ts)) WITH CLUSTERING ORDER BY (ts DESC);";

        readonly ISession session;

        public ActivityStreamsStorageManager(ISession session)
        {
            this.session = session;
        }

        public void CreateActivitiesStorage()
        {
            var createEventsTable = CreateEventsTableTemplate.ToLowerInvariant();
            session.Execute(createEventsTable);
        }
    }

    public class ProteusSerializer : ISerializer
    {
        Elders.Proteus.Serializer serializer;

        public ProteusSerializer(Assembly[] assembliesContainingContracts)
        {
            var internalAssemblies = assembliesContainingContracts.ToList();
            internalAssemblies.Add(typeof(Elders.Proteus.Serializer).Assembly);

            var identifier = new GuidTypeIdentifier(internalAssemblies.ToArray());
            serializer = new Elders.Proteus.Serializer(identifier);
        }

        public object Deserialize(System.IO.Stream str)
        {
            return serializer.DeserializeWithHeaders(str);
        }

        public void Serialize<T>(System.IO.Stream str, T message)
        {
            serializer.SerializeWithHeaders(str, message);
        }
    }

    public interface ISerializer
    {
        object Deserialize(Stream str);
        void Serialize<T>(Stream str, T message);
    }

    public static class ISerializerExtensions
    {
        public static object DeserializeFromBytes(this ISerializer self, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return self.Deserialize(stream);
            }
        }

        public static byte[] SerializeToBytes<T>(this ISerializer self, T message)
        {
            using (var stream = new MemoryStream())
            {
                self.Serialize(stream, message);
                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
