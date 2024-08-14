using e_commerce_course_api.DTOs;
using e_commerce_course_api.Helpers.Requests;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The repository for the products.
    /// </summary>
    public interface IProductRepository
    {
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
        /// Get the product brands.
        /// </summary>
        /// <returns>
        /// The product brands.
        /// </returns>
        Task<string[]> GetProductBrandsAsync();

        /// <summary>
        /// Get the product types.
        /// </summary>
        /// <returns>
        /// The product types.
        /// </returns>
        Task<string[]> GetProductTypesAsync();
    }
}
