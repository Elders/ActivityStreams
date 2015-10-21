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
    }
}
