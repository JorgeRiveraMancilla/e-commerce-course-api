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
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        /// <summary>
        /// Seeds the database.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// /// <param name="config">
        /// The configuration.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public static async Task SeedAsync(
            DataContext dataContext,
            UserManager<User> userManager,
            IConfiguration config
        )
        {
            await SeedProductsAsync(dataContext);
            await SeedUsersAsync(userManager, config);
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
        /// <param name="config">
        /// The configuration.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when admin name, email, or password is not found in the configuration.
        /// </exception>
        public static async Task SeedUsersAsync(
            UserManager<User> userManager,
            IConfiguration config
        )
        {
            if (userManager.Users.Any())
                return;

            string adminName =
                config["AdminUser:Name"] ?? throw new Exception("Admin name is required");
            string adminEmail =
                config["AdminUser:Email"] ?? throw new Exception("Admin email is required");
            string adminPassword =
                config["AdminUser:Password"] ?? throw new Exception("Admin password is required");

            var admin = new User { UserName = adminName, Email = adminEmail };
            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
            {
                var createdAdmin = await userManager.FindByNameAsync(adminName);
                if (createdAdmin != null)
                    await userManager.AddToRolesAsync(createdAdmin, ["Admin", "Member"]);
                else
                    throw new Exception("Failed to retrieve created admin user.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user. Errors: {errors}");
            }
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
