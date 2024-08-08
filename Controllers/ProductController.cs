using e_commerce_course_api.Data;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The controller for the products.
    /// </summary>
    /// <param name="dataContext">
    /// The data context to use.
    /// </param>
    public class ProductController(DataContext dataContext) : BaseApiController
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// Get the products.
        /// </summary>
        /// <param name="productParams">
        /// The parameters to use.
        /// </param>
        /// <returns>
        /// The products.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts(
            [FromQuery] ProductParams productParams
        )
        {
            var query = _dataContext
                .Products.Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(
                query,
                productParams.PageNumber,
                productParams.PageSize
            );

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
        /// The product.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _dataContext.Products.FindAsync(id);

            if (product == null)
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
            var brands = await _dataContext.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _dataContext.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}
