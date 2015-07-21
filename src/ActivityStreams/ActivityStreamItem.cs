namespace ActivityStreams
{
    public class ActivityStreamItem
    {
        ActivityStreamItem() { }

        public ActivityStreamItem(byte[] id, object body, object place, object author)
        {
            Id = id;
            Body = body;
            Place = place;
            Author = author;
        }

        public byte[] Id { get; private set; }

        public object Body { get; private set; }

        public object Place { get; private set; }

        public object Author { get; private set; }
    }

}
