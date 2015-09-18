namespace ActivityStreams.Persistence
{
    public interface IActivityFeedRepository
    {
        /// <summary>
        /// Gets the feed for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the feed.</param>
        /// <returns>Returns the feed.</returns>
        Feed Get(byte[] id);

        void Save(Feed feed);
    }
}
