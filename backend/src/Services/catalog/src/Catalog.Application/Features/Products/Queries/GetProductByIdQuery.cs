namespace Catalog.Application.Features.Products.Queries
{
    public class GetProductByIdQuery : IQuery<ProductItemResponse?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductItemResponse?>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repo;

        public GetProductByIdQueryHandler(IBaseRepositoryAsync<Product, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<ProductItemResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _repo.Entities
                .Include(c => c.Category)
                .Include(c => c.ProductImages)
                .Include(c => c.ProductSpecifications)
                .Include(c => c.ProductFacilities)
                .Include(c => c.ProductNears!).ThenInclude(nearItem => nearItem.ProductNearItems)
                .Select(product => new ProductItemResponse
                {
                    Id = product.Id,
                    UserId = product.UserId,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category!.Name,
                    SubCategoryId = product.SubCategoryId,
                    SubCategoryName = product.SubCategory!.Name,
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
                    ProductImages = product.ProductImages.Select(productProductImage => new ProductImageItemResponse
                    {
                        ProductId = productProductImage.ProductId,
                        ImageUrl = productProductImage.ImageUrl,
                        ImageType = productProductImage.ImageType,
                        ImageName = productProductImage.ImageName
                    }).ToList(),
                    ProductFacilities = product.ProductFacilities.Select(productProductFacility => new ProductFacilityResponse
                    {
                        ProductId = productProductFacility.ProductId,
                        FacilityId = productProductFacility.FacilityId,
                        FacilityName = productProductFacility.Facility.Name,
                    }).ToList(),
                    ProductNears = product.ProductNears.Select(productProductNear => new ProductNearResponse
                    {
                        ProductId = productProductNear.ProductId,
                        Title = productProductNear.Title,
                        ProductNearItems = productProductNear.ProductNearItems.Select(productNearItem => new ProductNearItemResponse
                        {
                            ProductId = productNearItem.ProductId,
                            ProductNearId = productNearItem.ProductNearId,
                            Title = productNearItem.Title
                        }).ToList(),
                    }).ToList(),
                    ProductSpecifications = product.ProductSpecifications!.Select(productProductSpecification => new ProductSpecificationResponse
                    {
                        ProductId = productProductSpecification.ProductId,
                        Title = productProductSpecification.Title,
                        Description = productProductSpecification.Description
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken: cancellationToken);

            //var item = MapToProductItemResponse(product);
            return product;
        }

        private static ProductItemResponse? MapToProductItemResponse(Product? product)
        {
            return product != null ? new ProductItemResponse
            {
                Id = product.Id,
                UserId = product.UserId,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                SubCategoryId = product.SubCategoryId,
                SubCategoryName = product.SubCategory?.Name,
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
                    FacilityId = productProductFacility.FacilityId,
                    FacilityName = productProductFacility.Facility?.Name
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
            } : null;
        }
    }
}