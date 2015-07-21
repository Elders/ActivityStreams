using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Streams
{

    [Subject("Streams")]
    public class When_comparing_two_different_activity_stream_items
    {
        Establish context = () =>
            {
                var id1 = Encoding.UTF8.GetBytes("ActivityStreamItemId1");
                item1 = new ActivityStreamItem(id1, "body1", "place1", "author1");

                var id2 = Encoding.UTF8.GetBytes("ActivityStreamItemId2");
                item2 = new ActivityStreamItem(id2, "body2", "place2", "author2");
            };

        Because of = () => areEqual = item1.Equals(item2) && item1 == item2;

        It should_not_be_equal = () => areEqual.ShouldBeFalse();

        static bool areEqual = false;
        static ActivityStreamItem item1;
        static ActivityStreamItem item2;
    }
}