namespace ActivityStreams
{
    public class FeedOptions
    {
        public FeedOptions(Paging paging, SortOrder sortOrder)
        {
            Paging = paging;
            SortOrder = sortOrder;
        }

        public Paging Paging { get; set; }
        public SortOrder SortOrder { get; set; }

        public static FeedOptions Default = new FeedOptions(Paging.Default, SortOrder.Descending);
    }
}
