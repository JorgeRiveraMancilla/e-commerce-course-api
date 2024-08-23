using e_commerce_course_api.Entities;
using e_commerce_course_api.Entities.Baskets;
using e_commerce_course_api.Entities.Orders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Data
{
    /// <summary>
    /// The data context.
    /// </summary>
    public class DataContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
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

        /// <summary>
        /// The orders in the data context.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// The order items in the data context.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// The order statuses in the data context.
        /// </summary>
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        /// <summary>
        /// The addresses in the data context.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Overrides the method to configure the roles.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Role>()
                .HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Member",
                        NormalizedName = "MEMBER"
                    }
                );
        }
    }
}
