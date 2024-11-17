using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_commerce_course_api.DTOs.Products;
using e_commerce_course_api.Entities;
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
    /// <param name="photoService">
    /// The photo service to use.
    /// </param>
    public class ProductRepository(
        DataContext dataContext,
        IMapper mapper,
        IPhotoService photoService
    ) : IProductRepository
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
        /// The photo service to use.
        /// </summary>
        private readonly IPhotoService _photoService = photoService;

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
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var imageResult = await _photoService.AddImageAsync(createProductDto.File);

            if (imageResult.Error is not null)
                throw new Exception(imageResult.Error.Message);

            var product = _mapper.Map<Product>(
                createProductDto,
                opt =>
                {
                    opt.AfterMap((src, dest) => dest.ImageUrl = imageResult.SecureUrl.ToString());
                    opt.AfterMap((src, dest) => dest.PublicId = imageResult.PublicId);
                }
            );

            _ = await _dataContext.Products.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }

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
        public async Task DeleteProductByIdAsync(int id)
        {
            var product =
                await _dataContext.Products.FindAsync(id)
                ?? throw new Exception("Producto no encontrado.");

            if (!string.IsNullOrEmpty(product.PublicId))
                _ = await _photoService.DeleteImageAsync(product.PublicId);

            _ = _dataContext.Products.Remove(product);
        }

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
        /// Check if a product exists by id.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// A boolean indicating whether the product exists.
        /// </returns>
        public async Task<bool> ProductExistsByIdAsync(int id)
        {
            return await _dataContext.Products.AnyAsync(x => x.Id == id);
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
        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var product =
                await _dataContext.Products.FindAsync(updateProductDto.Id)
                ?? throw new Exception("Producto no encontrado.");

            _ = _mapper.Map(updateProductDto, product);

            if (updateProductDto is not null)
            {
                var imageResult = await _photoService.AddImageAsync(updateProductDto.File);

                if (imageResult.Error is not null)
                    throw new Exception(imageResult.Error.Message);

                if (!string.IsNullOrEmpty(product.PublicId))
                    _ = await _photoService.DeleteImageAsync(product.PublicId);

                product.ImageUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            return _mapper.Map<ProductDto>(product);
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
