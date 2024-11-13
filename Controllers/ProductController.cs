using e_commerce_course_api.DTOs.Products;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Helpers.Requests;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("{id}", Name = "GetProduct")]
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

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="createProductDto">
        /// The product to create.
        /// </param>
        /// <returns>
        /// The product created.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(
            [FromForm] CreateProductDto createProductDto
        )
        {
            try
            {
                var product = await _productRepository.CreateProductAsync(createProductDto);

                if (await _productRepository.SaveChangesAsync())
                    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
            }

            return BadRequest(new ProblemDetails { Title = "Error al crear el producto" });
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="productDto">
        /// The product to update.
        /// </param>
        /// <returns>
        /// The product updated.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ProductDto>> UpdateProduct(
            [FromForm] UpdateProductDto productDto
        )
        {
            var exists = await _productRepository.ProductExistsByIdAsync(productDto.Id);

            if (!exists)
                return NotFound();

            try
            {
                var product = await _productRepository.UpdateProductAsync(productDto);

                if (await _productRepository.SaveChangesAsync())
                    return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
            }

            return BadRequest(new ProblemDetails { Title = "Error al actualizar el producto" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProductByIdAsync(id);

            if (await _productRepository.SaveChangesAsync())
                return Ok();

            return BadRequest(new ProblemDetails { Title = "Error al eliminar el producto" });
        }
    }
}
