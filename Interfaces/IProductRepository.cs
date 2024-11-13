using e_commerce_course_api.DTOs.Products;
using e_commerce_course_api.Helpers.Requests;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The repository for the products.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="createProductDto">
        /// The product to create.
        /// </param>
        /// <returns>
        /// The created product.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the image upload fails.
        /// </exception>
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);

        /// <summary>
        /// Delete a product by id.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the product is not found.
        /// </exception>
        Task DeleteProductByIdAsync(int id);

        /// <summary>
        /// Get the product brands.
        /// </summary>
        /// <returns>
        /// The product brands.
        /// </returns>
        Task<string[]> GetProductBrandsAsync();

        /// <summary>
        /// Get a product by id.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// The product.
        /// </returns>
        Task<ProductDto?> GetProductByIdAsync(int id);

        /// <summary>
        /// Get the products.
        /// </summary>
        /// <param name="productParams">
        /// The parameters to use.
        /// </param>
        /// <returns>
        /// The products.
        /// </returns>
        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);

        /// <summary>
        /// Get the product types.
        /// </summary>
        /// <returns>
        /// The product types.
        /// </returns>
        Task<string[]> GetProductTypesAsync();

        /// <summary>
        /// Check if a product exists by id.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// A boolean indicating whether the product exists.
        /// </returns>
        Task<bool> ProductExistsByIdAsync(int id);

        /// <summary>
        /// Save the changes.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the changes were saved.
        /// </returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="updateProductDto">
        /// The product to update.
        /// </param>
        /// <returns>
        /// The updated product.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the product is not found.
        /// </exception>
        Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto);

        /// <summary>
        /// Update the stock of a product.
        /// </summary>
        /// <param name="productId">
        /// The product identifier.
        /// </param>
        /// <param name="quantity">
        /// The quantity to update the stock by.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the product is not found.
        /// </exception>
        Task<ProductDto> UpdateStockAsync(int productId, int quantity);
    }
}
