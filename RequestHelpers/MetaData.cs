namespace e_commerce_course_api.RequestHelpers
{
    /// <summary>
    /// Represents the metadata for a paged list.
    /// </summary>
    public class MetaData
    {
        /// <summary>
        /// The current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
