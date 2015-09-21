using System.Reflection;
using System.Text;
using Cassandra;
using Machine.Specifications;
using System.Runtime.Serialization;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_something
    {
        Establish context = () =>
        {
            var id = Encoding.UTF8.GetBytes("activityId");
            var streamId = Encoding.UTF8.GetBytes("streamId");
            var body = new ActivityBody() { GG = "gg" };
            activity = new Activity(id, streamId, body, "author");

            var session = SessionCreator.Create();
            var serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            repo = new ActivityRepository(session, serializer);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateActivitiesStorage();
        };

        Because of = () => repo.Append(activity);

        It should_ = () => 123.ShouldBeOfExactType<int>();

        static IActivityRepository repo;
        static Activity activity;
    }

    public class SessionCreator
    {
        public static ISession Create()
        {
            var cluster = Cluster.Builder()
                .WithConnectionString("Contact Points=10.10.63.27;Port=9042;Default Keyspace=test123")
                .Build();
            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
            return session;
        }
    }

    [DataContract(Name = "92e17681-1468-4297-9818-6e70cd97d20a")]
    public class ActivityBody
    {
        [DataMember(Order = 1)]
        public string GG { get; set; }
    }

}
