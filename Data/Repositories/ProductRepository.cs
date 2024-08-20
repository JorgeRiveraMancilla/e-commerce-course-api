using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Helpers.Requests;
using e_commerce_course_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Data.Repositories
{
    /// <summary>
    /// The repository for the products.
    /// </summary>
    /// <param name="dataContext">
    /// The data context to use.
    /// </param>
    /// <param name="mapper">
    /// The mapper to use.
    /// </param>
    public class ProductRepository(DataContext dataContext, IMapper mapper) : IProductRepository
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// The mapper to use.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get the product brands.
        /// </summary>
        /// <returns>
        /// The product brands.
        /// </returns>
        public async Task<string[]> GetProductBrandsAsync()
        {
            return await _dataContext.Products.Select(x => x.Brand).Distinct().ToArrayAsync();
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
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            return await _dataContext
                .Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get the products.
        /// </summary>
        /// <param name="productParams">
        /// The parameters to use.
        /// </param>
        /// <returns>
        /// The products.
        /// </returns>
        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
        {
            var query = _dataContext
                .Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var orderBy = productParams.OrderBy?.Trim().ToLower();
            var searchTerm = productParams.SearchTerm?.Trim().ToLower();
            var brands = productParams.Brands?.Trim().ToLower();
            var types = productParams.Types?.Trim().ToLower();
            var brandList = new List<string>();
            var typeList = new List<string>();

            query = orderBy switch
            {
                "price-asc" => query.OrderBy(x => x.Price),
                "price-desc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
#pragma warning restore CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons

            if (!string.IsNullOrWhiteSpace(brands))
            {
                brandList.AddRange([.. brands.Split(",")]);
                query = query.Where(p => brandList.Contains(p.Brand.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(types))
            {
                typeList.AddRange([.. types.Split(",")]);
                query = query.Where(p => typeList.Contains(p.Type.ToLower()));
            }

            return await PagedList<ProductDto>.ToPagedList(
                query,
                productParams.PageNumber,
                productParams.PageSize
            );
        }

        /// <summary>
        /// Get the product types.
        /// </summary>
        /// <returns>
        /// The product types.
        /// </returns>
        public async Task<string[]> GetProductTypesAsync()
        {
            return await _dataContext.Products.Select(x => x.Type).Distinct().ToArrayAsync();
        }

        /// <summary>
        /// Save the changes.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the changes were saved.
        /// </returns>
        public async Task<bool> SaveChangesAsync()
        {
            return 0 < await _dataContext.SaveChangesAsync();
        }

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
        public async Task<ProductDto> UpdateStockAsync(int productId, int quantity)
        {
            var product =
                await _dataContext.Products.FindAsync(productId)
                ?? throw new Exception("Producto no encontrado.");

            product.Stock += quantity;

            return _mapper.Map<ProductDto>(product);
        }
    }
}
