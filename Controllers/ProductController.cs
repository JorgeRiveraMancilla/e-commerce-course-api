using e_commerce_course_api.DTOs;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Helpers.Requests;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The controller for the products.
    /// </summary>
    /// <param name="productRepository">
    /// The repository for the products.
    /// </param>
    public class ProductController(IProductRepository productRepository) : BaseApiController
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        /// Get the products.
        /// </summary>
        /// <param name="productParams">
        /// The parameters to use.
        /// </param>
        /// <returns>
        /// The products.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts(
            [FromQuery] ProductParams productParams
        )
        {
            var products = await _productRepository.GetProductsAsync(productParams);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        /// <summary>
        /// Get a product by id.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// The product if exists, otherwise not found.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
                return NotFound();

            return product;
        }

        /// <summary>
        /// Get the filters.
        /// </summary>
        /// <returns>
        /// The filters.
        /// </returns>
        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _productRepository.GetProductBrandsAsync();
            var types = await _productRepository.GetProductTypesAsync();

            return Ok(new { brands, types });
        }
    }
}
