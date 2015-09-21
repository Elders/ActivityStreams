using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_a_new_activity_is_created
    {
        Establish context = () =>
            {
                id = Encoding.UTF8.GetBytes("activityId");
                streamId = Encoding.UTF8.GetBytes("streamId");
                body = "body";
                author = "author";
            };

        Because of = () => item = new Activity(id, streamId, body, author);

        It should_have_id = () => item.ExternalId.ShouldEqual(id);
        It should_have_stream_id = () => item.StreamId.ShouldEqual(streamId);
        It should_have_author = () => item.Author.ShouldEqual(author);
        It should_have_body = () => item.Body.ShouldEqual(body);

        static Activity item;
        static byte[] id;
        static byte[] streamId;
        static object body;
        static string author;
    }
}
