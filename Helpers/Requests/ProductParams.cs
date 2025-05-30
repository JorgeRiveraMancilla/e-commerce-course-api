namespace e_commerce_course_api.Helpers.Requests
{
    /// <summary>
    /// Represents the parameters for a product request.
    /// </summary>
    public class ProductParams : PaginationParams
    {
        /// <summary>
        /// The order by clause.
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// The search term.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// The types.
        /// </summary>
        public string? Types { get; set; }

        /// <summary>
        /// The brands.
        /// </summary>
        public string? Brands { get; set; }
    }
}
