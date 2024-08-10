using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Helpers.Requests
{
    /// <summary>
    /// Represents the metadata for a paged list.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items in the paged list.
    /// </typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// The metadata for the paged list.
        /// </summary>
        public MetaData MetaData { get; set; }

        /// <summary>
        /// Creates a new paged list.
        /// </summary>
        /// <param name="items">
        /// The items to be paged.
        /// </param>
        /// <param name="count">
        /// The total number of items.
        /// </param>
        /// <param name="pageNumber">
        /// The current page number.
        /// </param>
        /// <param name="pageSize">
        /// The number of items per page.
        /// </param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        /// <summary>
        /// Converts a queryable to a paged list.
        /// </summary>
        /// <param name="query">
        /// The queryable to be paged.
        /// </param>
        /// <param name="pageNumber">
        /// The current page number.
        /// </param>
        /// <param name="pageSize">
        /// The number of items per page.
        /// </param>
        /// <returns>
        /// The paged list.
        /// </returns>
        public static async Task<PagedList<T>> ToPagedList(
            IQueryable<T> query,
            int pageNumber,
            int pageSize
        )
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
