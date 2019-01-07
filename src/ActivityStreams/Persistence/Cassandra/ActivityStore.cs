using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        const string RemoveActivityStreamQueryTemplateDesc = @"DELETE FROM ""activities_desc"" where sid=? AND ts=?;";
        const string RemoveActivityStreamQueryTemplateAsc = @"DELETE FROM ""activities_asc"" where sid=? AND ts=?;";

        private readonly ISerializer serializer;

        private readonly ICassandraProvider cassandraProvider;

        private ISession GetSession() => cassandraProvider.GetSession();

        public ActivityStore(ICassandraProvider cassandraProvider, ISerializer serializer)
        {
            this.cassandraProvider = cassandraProvider ?? throw new ArgumentNullException(nameof(cassandraProvider));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void Save(Activity activity)
        {
            var preparedAppendDesc = GetSession().Prepare(AppendActivityStreamQueryTemplateDesc);
            var preparedAppendAsc = GetSession().Prepare(AppendActivityStreamQueryTemplateAsc);

            byte[] data = SerializeActivity(activity);
            GetSession()
                .Execute(preparedAppendDesc
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));

            GetSession()
                .Execute(preparedAppendAsc
                .Bind(Convert.ToBase64String(activity.StreamId), activity.Timestamp, data));
        }

        public void Delete(byte[] streamId, long timestamp)
        {
            var preparedRemoveDesc = GetSession().Prepare(RemoveActivityStreamQueryTemplateDesc);
            var preparedRemoveAsc = GetSession().Prepare(RemoveActivityStreamQueryTemplateAsc);

            GetSession()
                .Execute(preparedRemoveDesc
                .Bind(Convert.ToBase64String(streamId), timestamp));

            GetSession()
                .Execute(preparedRemoveAsc
                .Bind(Convert.ToBase64String(streamId), timestamp));
        }

        public IEnumerable<Activity> LoadStream(byte[] streamId, ActivityStreamOptions options)
        {
            options = options ?? ActivityStreamOptions.Default;

            SortedSet<Activity> activities = new SortedSet<Activity>(Activity.ComparerDesc);

            var sortOrder = options.SortOrder;
            var paging = options.Paging;

            if (sortOrder == SortOrder.Ascending)
            {
                activities = new SortedSet<Activity>(Activity.ComparerAsc);
            }

            var streamIdQuery = Convert.ToBase64String(streamId);

            PreparedStatement prepared = GetPreparedStatementToLoadActivityStream(sortOrder == SortOrder.Ascending);
            var query = prepared
                    .Bind(streamIdQuery, paging.Timestamp)
                    .SetAutoPage(false)
                    .SetPageSize(paging.Take);

            var rowSet = GetSession().Execute(query);
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

        public async Task<IEnumerable<Activity>> LoadStreamAsync(byte[] streamId, ActivityStreamOptions options)
        {
            options = options ?? ActivityStreamOptions.Default;

            SortedSet<Activity> activities = new SortedSet<Activity>(Activity.ComparerDesc);

            var sortOrder = options.SortOrder;
            var paging = options.Paging;

            if (sortOrder == SortOrder.Ascending)
            {
                activities = new SortedSet<Activity>(Activity.ComparerAsc);
            }

            var streamIdQuery = Convert.ToBase64String(streamId);

            PreparedStatement prepared = await GetPreparedStatementToLoadActivityStreamAsync(sortOrder == SortOrder.Ascending).ConfigureAwait(false);

            var query = prepared
                    .Bind(streamIdQuery, paging.Timestamp)
                    .SetAutoPage(false)
                    .SetPageSize(paging.Take);

            var rowSet = await GetSession().ExecuteAsync(query).ConfigureAwait(false);
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

        PreparedStatement loadActivityStreamAsc_PreparedStatement;
        PreparedStatement loadActivityStreamDesc_PreparedStatement;

        PreparedStatement GetPreparedStatementToLoadActivityStream(bool ascending)
        {
            if (ascending)
            {
                if (loadActivityStreamAsc_PreparedStatement is null)
                    loadActivityStreamAsc_PreparedStatement = GetSession().Prepare(LoadActivityStreamQueryTemplateAsc);

                return loadActivityStreamAsc_PreparedStatement;
            }
            else
            {
                if (loadActivityStreamDesc_PreparedStatement is null)
                    loadActivityStreamDesc_PreparedStatement = GetSession().Prepare(LoadActivityStreamQueryTemplateDesc);

                return loadActivityStreamDesc_PreparedStatement;
            }
        }

        async Task<PreparedStatement> GetPreparedStatementToLoadActivityStreamAsync(bool ascending)
        {
            if (ascending)
            {
                if (loadActivityStreamAsc_PreparedStatement is null)
                    loadActivityStreamAsc_PreparedStatement = await GetSession().PrepareAsync(LoadActivityStreamQueryTemplateAsc).ConfigureAwait(false);

                return loadActivityStreamAsc_PreparedStatement;
            }
            else
            {
                if (loadActivityStreamDesc_PreparedStatement is null)
                    loadActivityStreamDesc_PreparedStatement = await GetSession().PrepareAsync(LoadActivityStreamQueryTemplateDesc).ConfigureAwait(false);

                return loadActivityStreamDesc_PreparedStatement;
            }
        }
    }
}
