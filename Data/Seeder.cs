using System.Text.Json;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Data
{
    /// <summary>
    /// Represents a seeder.
    /// </summary>
    public static class Seeder
    {
        /// <summary>
        /// The options for the JSON serializer.
        /// </summary>
        private static readonly JsonSerializerOptions _options =
            new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Seeds the database.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public static async Task SeedAsync(DataContext dataContext, UserManager<User> userManager)
        {
            await SeedProductsAsync(dataContext);
            await SeedUsersAsync(userManager);
            await SeedOrderStatusesAsync(dataContext);
        }

        /// <summary>
        /// Seeds the products.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public static async Task SeedProductsAsync(DataContext dataContext)
        {
            if (dataContext.Products.Any())
                return;

            var jsonData = await File.ReadAllTextAsync("Data/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(jsonData, _options);

            if (products == null)
                return;

            await dataContext.AddRangeAsync(products);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Seeds the users.
        /// </summary>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            if (userManager.Users.Any())
                return;

            var user = new User { UserName = "bob", Email = "bob@test.com" };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

            var admin = new User { UserName = "admin", Email = "admin@test.com" };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, ["Admin", "Member"]);
        }

        /// <summary>
        /// Seeds the order statuses.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public static async Task SeedOrderStatusesAsync(DataContext dataContext)
        {
            if (dataContext.OrderStatuses.Any())
                return;

            var orderStatuses = new List<OrderStatus>
            {
                new() { Name = "Pending" },
                new() { Name = "Payment Received" },
                new() { Name = "Payment Failed" },
            };

            await dataContext.AddRangeAsync(orderStatuses);
            await dataContext.SaveChangesAsync();
        }
    }
}
