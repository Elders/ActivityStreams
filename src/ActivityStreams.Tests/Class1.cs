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
    public class When_a_new_activity_stream_item_is_appended_to_a_stream
    {
        It should_be_available_as_the_most_recent_item_of_the_stream;
    }
}
