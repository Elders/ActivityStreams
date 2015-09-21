using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cassandra;
using Elders.Proteus;

namespace ActivityStreams.Persistence.Cassandra
{
    public class ActivityRepository : IActivityRepository
    {
        const string AppendActivityQueryTemplate = @"INSERT INTO ""activities_desc"" (sid,ts,data) VALUES (?,?,?);";

        readonly ISerializer serializer;

        readonly ISession session;

        public ActivityRepository(ISession session, ISerializer serializer)
        {
            this.session = session;
            this.serializer = serializer;
        }

        public void Append(Activity activity)
        {
            var prepared = session.Prepare(AppendActivityQueryTemplate);

            byte[] data = SerializeActivity(activity);
            session
                .Execute(prepared
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));
        }

        byte[] SerializeActivity(Activity activity)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, activity);
                return stream.ToArray();
            }
        }

        public IEnumerable<Activity> Load(Feed feed)
        {
            throw new NotImplementedException();
        }
    }

    public class ActivityStore
    {

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
