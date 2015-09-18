using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_comparing_two_equal_activities
    {
        Establish context = () =>
            {
                var streamId = Encoding.UTF8.GetBytes("streamId");

                var id1 = Encoding.UTF8.GetBytes("ActivityStreamItemId1");
                item1 = new Activity(id1, streamId, "body1", "author1");

                var id2 = Encoding.UTF8.GetBytes("ActivityStreamItemId1");
                item2 = new Activity(id2, streamId, "body2", "author2");
            };

        Because of = () => areEqual = item1.Equals(item2) && item1 == item2;

        It should_be_equal = () => areEqual.ShouldBeTrue();

        static bool areEqual = false;
        static Activity item1;
        static Activity item2;
    }
}
