using System;
using System.Collections.Generic;
using System.IO;
using Cassandra;

namespace ActivityStreams.Persistence.Cassandra
{
    /// <summary>
    /// Type reference: http://docs.datastax.com/en/developer/csharp-driver/2.7/csharp-driver/reference/csharpType2Cql3Datatypes_r.html
    /// </summary>
    public class ActivityStore : IActivityStore
    {
        const string AppendActivityStreamQueryTemplateDesc = @"INSERT INTO activities_desc (sid,ts,data) VALUES (?,?,?);";

        const string AppendActivityStreamQueryTemplateAsc = @"INSERT INTO activities_asc (sid,ts,data) VALUES (?,?,?);";

        const string LoadActivityStreamQueryTemplateDesc = @"SELECT data FROM ""activities_desc"" where sid=? AND ts<=?;";

        const string LoadActivityStreamQueryTemplateAsc = @"SELECT data FROM ""activities_asc"" where sid=? AND ts<=?;";

        readonly ISerializer serializer;

        readonly ISession session;

        public ActivityStore(ISession session, ISerializer serializer)
        {
            this.session = session;
            this.serializer = serializer;
        }

        public void Save(Activity activity)
        {
            var preparedAppendDesc = session.Prepare(AppendActivityStreamQueryTemplateDesc);
            var preparedAppendAsc = session.Prepare(AppendActivityStreamQueryTemplateAsc);

            byte[] data = SerializeActivity(activity);
            session
                .Execute(preparedAppendDesc
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));

            session
                .Execute(preparedAppendAsc
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));
        }

        public IEnumerable<Activity> LoadStream(byte[] streamId, ActivityStreamOptions options)
        {
            options = options ?? ActivityStreamOptions.Default;

            var statement = LoadActivityStreamQueryTemplateDesc;
            SortedSet<Activity> activities = new SortedSet<Activity>(Activity.ComparerDesc);

            var sortOrder = options.SortOrder;
            var paging = options.Paging;

            if (sortOrder == SortOrder.Ascending)
            {
                statement = LoadActivityStreamQueryTemplateAsc;
                activities = new SortedSet<Activity>(Activity.ComparerAsc);
            }

            var streamIdQuery = Convert.ToBase64String(streamId);

            var prepared = session
                    .Prepare(statement)
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

            return activities;
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
