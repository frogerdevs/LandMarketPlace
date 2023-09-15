namespace Catalog.Application.Features.Products.Queries
{
    public class GetProductsByCategorySlugQuery : IQuery<BasePagingResponse<ProductsByCategoryItemResponse>>
    {
        public required string Slug { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public sealed class GetProductsByCategorySlugQueryHandler : IQueryHandler<GetProductsByCategorySlugQuery, BasePagingResponse<ProductsByCategoryItemResponse>>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repoProduct;
        private readonly IBaseRepositoryAsync<Category, string> _repoCategory;

        public GetProductsByCategorySlugQueryHandler(IBaseRepositoryAsync<Product, string> repoProduct, IBaseRepositoryAsync<Category, string> repoCategory)
        {
            _repoCategory = repoCategory;
            _repoProduct = repoProduct;
        }

        public async ValueTask<BasePagingResponse<ProductsByCategoryItemResponse>> Handle(GetProductsByCategorySlugQuery query, CancellationToken cancellationToken)
        {
            var response = new BasePagingResponse<ProductsByCategoryItemResponse>
            {
                Limit = query.PageSize,
                CurrentPage = query.PageNumber,
                Success = false,
                Message = "Failed Get Data",
                TotalData = 0,
                Count = 0,
                Data = null
            };
            var category = await _repoCategory.NoTrackingEntities
                .FirstOrDefaultAsync(c => c.Active && c.Slug == query.Slug, cancellationToken: cancellationToken);

            if (category != null)
            {
                var queryRepo = _repoProduct.Entities.Include(c => c.Category)
                .Include(c => c.ProductImages).AsQueryable()
                .Where(c => c.CategoryId == category.Id && c.Active);

                response.TotalData = await queryRepo.CountAsync(cancellationToken: cancellationToken);

                var items = await queryRepo
                    .Select(c => new ProductsByCategoryItemResponse
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        CategoryId = c.CategoryId,
                        CategoryName = c.Category != null ? c.Category.Name : null,
                        CategorySlug = c.Category != null ? c.Category.Slug : null,
                        Title = c.Title,
                        Slug = c.Slug,
                        Province = c.Province,
                        City = c.City,
                        District = c.District,
                        PriceFrom = c.PriceFrom,
                        PriceTo = c.PriceTo,
                        Active = c.Active,
                        ImageUrl = c.ProductImages != null ? c.ProductImages.Where(i => i.ProductId == c.Id).Select(i => i.ImageUrl).FirstOrDefault() : null
                    })
                .AsNoTracking()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken: cancellationToken);

                response.Count = items.Count;
                response.Data = items;
                response.Success = true;
                response.Message = "Success Get Data";

                return response;
            }
            return response;
        }
        private static ProductItemResponse MapToProductItemResponse(Product product)
        {
            return new ProductItemResponse
            {
                Id = product.Id,
                UserId = product.UserId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Title = product.Title,
                Slug = product.Slug,
                Province = product.Province,
                City = product.City,
                District = product.District,
                SubDistrict = product.SubDistrict,
                PostCode = product.PostCode,
                Address = product.Address,
                CertificateId = product.CertificateId,
                RegisteredSince = product.RegisteredSince,
                PriceFrom = product.PriceFrom,
                PriceTo = product.PriceTo,
                Description = product.Description,
                Details = product.Details,
                LocationLongitude = product.LocationLongitude,
                LocationLatitude = product.LocationLatitude,
                Active = product.Active,
                ProductImages = product.ProductImages?.Select(productProductImage => new ProductImageItemResponse
                {
                    ProductId = productProductImage.ProductId,
                    ImageUrl = productProductImage.ImageUrl,
                    ImageType = productProductImage.ImageType,
                    ImageName = productProductImage.ImageName
                }).ToList(),
                ProductFacilities = product.ProductFacilities?.Select(productProductFacility => new ProductFacilityResponse
                {
                    ProductId = productProductFacility.ProductId,
                    FacilityId = productProductFacility.FacilityId
                }).ToList(),
                ProductNears = product.ProductNears?.Select(productProductNear => new ProductNearResponse
                {
                    ProductId = productProductNear.ProductId,
                    Title = productProductNear.Title,
                    ProductNearItems = productProductNear.ProductNearItems?.Select(productProductNearProductNearItem => new ProductNearItemResponse
                    {
                        ProductId = productProductNearProductNearItem.ProductId,
                        ProductNearId = productProductNearProductNearItem.ProductNearId,
                        Title = productProductNearProductNearItem.Title
                    }).ToList()
                }).ToList(),
                ProductSpecifications = product.ProductSpecifications?.Select(productProductSpecification => new ProductSpecificationResponse
                {
                    ProductId = productProductSpecification.ProductId,
                    Title = productProductSpecification.Title,
                    Description = productProductSpecification.Description
                }).ToList()
            };
        }

    }

}
