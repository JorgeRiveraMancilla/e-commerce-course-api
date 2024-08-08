using e_commerce_course_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Data
{
    /// <summary>
    /// Represents the data context of the application.
    /// </summary>
    /// <param name="options">
    /// The options of the data context.
    /// </param>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        /// <summary>
        /// The products in the data context.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// The baskets in the data context.
        /// </summary>
        public DbSet<Basket> Baskets { get; set; }

        /// <summary>
        /// The basket items in the data context.
        /// </summary>
        public DbSet<BasketItem> BasketItems { get; set; }
    }
}
