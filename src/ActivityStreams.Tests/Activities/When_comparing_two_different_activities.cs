using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_comparing_two_different_activities
    {
        Establish context = () =>
            {
                var streamId = Encoding.UTF8.GetBytes("streamId");

                var id1 = Encoding.UTF8.GetBytes("ActivityId1");
                item1 = new Activity(streamId, id1, "body1", "author1");

                var id2 = Encoding.UTF8.GetBytes("ActivityId2");
                item2 = new Activity(streamId, id2, "body1", "author1");
            };

        Because of = () => areEqual = item1.Equals(item2) && item1 == item2;

        It should_not_be_equal = () => areEqual.ShouldBeFalse();

        static bool areEqual = false;
        static Activity item1;
        static Activity item2;
    }
}
