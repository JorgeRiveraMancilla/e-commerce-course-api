using e_commerce_course_api.Entities;

namespace e_commerce_course_api.Extensions
{
    /// <summary>
    /// Contains extension methods for the Product entity.
    /// </summary>
    public static class ProductExtensions
    {
        /// <summary>
        /// Sorts the products based on the provided order by parameter.
        /// </summary>
        /// <param name="query">
        /// The query to be sorted.
        /// </param>
        /// <param name="orderBy">
        /// The order by parameter.
        /// </param>
        /// <returns>
        /// The sorted query.
        /// </returns>
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string? orderBy)
        {
            orderBy = orderBy?.Trim();

            if (string.IsNullOrWhiteSpace(orderBy))
                return query.OrderBy(p => p.Name);

            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(n => n.Name)
            };

            return query;
        }

        /// <summary>
        /// Searches the products based on the provided search term.
        /// </summary>
        /// <param name="query">
        /// The query to be searched.
        /// </param>
        /// <param name="searchTerm">
        /// The search term.
        /// </param>
        /// <returns>
        /// The searched query.
        /// </returns>
        public static IQueryable<Product> Search(this IQueryable<Product> query, string? searchTerm)
        {
            searchTerm = searchTerm?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            return query.Where(p =>
                p.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        /// <summary>
        /// Filters the products based on the provided brands and types.
        /// </summary>
        /// <param name="query">
        /// The query to be filtered.
        /// </param>
        /// <param name="brands">
        /// The brands to filter by.
        /// </param>
        /// <param name="types">
        /// The types to filter by.
        /// </param>
        /// <returns>
        /// The filtered query.
        /// </returns>
        public static IQueryable<Product> Filter(
            this IQueryable<Product> query,
            string? brands,
            string? types
        )
        {
            var brandList = new List<string>();
            var typeList = new List<string>();

            if (!string.IsNullOrWhiteSpace(brands))
            {
                brandList.AddRange([.. brands.ToLower().Split(",")]);
            }

            query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));

            if (!string.IsNullOrWhiteSpace(types))
            {
                typeList.AddRange([.. types.ToLower().Split(",")]);
            }

            query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()));

            return query;
        }
    }
}
