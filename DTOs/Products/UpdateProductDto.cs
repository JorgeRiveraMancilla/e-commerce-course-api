using System.ComponentModel.DataAnnotations;

namespace e_commerce_course_api.DTOs.Products
{
    /// <summary>
    /// The data transfer object for updating a product.
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// The id of the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The price of the product.
        /// </summary>
        [Range(1, double.PositiveInfinity)]
        public required long Price { get; set; }

        /// <summary>
        /// The image of the product.
        /// </summary>
        public required IFormFile File { get; set; }

        /// <summary>
        /// The type of the product.
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// The brand of the product.
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// The quantity of the product in stock.
        /// </summary>
        [Range(0, 200)]
        public required int Stock { get; set; }
    }
}
