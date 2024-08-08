using System.Text.Json;
using e_commerce_course_api.Entities;

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
        /// Seeds the products.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
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
    }
}
