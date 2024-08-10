namespace e_commerce_course_api.Helpers.Requests
{
    /// <summary>
    /// Represents the parameters for pagination.
    /// </summary>
    public class PaginationParams
    {
        /// <summary>
        /// The maximum page size.
        /// </summary>
        private const int maxPageSize = 50;

        /// <summary>
        /// The current page number.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of items per page.
        /// </summary>
        private int _pageSize = 6;

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}
