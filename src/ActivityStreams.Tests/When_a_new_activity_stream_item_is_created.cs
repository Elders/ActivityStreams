using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests
{
    [Subject("Streams")]
    public class When_a_new_activity_stream_item_is_created
    {
        Establish context = () =>
            {
                id = Encoding.UTF8.GetBytes("ActivityStreamItemId");
                body = "body";
                place = "place";
                author = "author";
            };

        Because of = () => item = new ActivityStreamItem(id, body, place, author);

        It should_have_id = () => item.Id.ShouldEqual(id);
        It should_have_author = () => item.Author.ShouldEqual(author);
        It should_have_body = () => item.Body.ShouldEqual(body);
        It should_have_place = () => item.Place.ShouldEqual(place);

        static ActivityStreamItem item;
        static byte[] id;
        static object body;
        static object place;
        static object author;

    }
}