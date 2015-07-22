using System.Linq;
using System.Text;
using Machine.Specifications;

namespace ActivityStreams.Tests
{

    [Subject("Streams")]
    public class When_a_new_stream_is_created
    {
        It should_contain_at_least_one_item;
    }

    [Subject("Streams")]
    public class When_a_new_activity_is_appended_to_a_stream
    {
        Establish context = () =>
            {
                stream = new ActivityStream();

                var streamId = Encoding.UTF8.GetBytes("streamId");
                var id = Encoding.UTF8.GetBytes("activityId");
                activity = new Activity(id, streamId, "body", "author");
            };

        Because of = () => stream.Append(activity);

        It should_be_available_as_the_most_recent_activity_of_the_stream = () => stream.Activities.First().ShouldEqual(activity);

        static ActivityStream stream;
        static Activity activity;
    }
}
