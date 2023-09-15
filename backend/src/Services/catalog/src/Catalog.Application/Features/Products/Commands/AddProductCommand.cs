namespace Catalog.Application.Features.Products.Commands
{
    public partial class AddProductCommand : ICommand<AddProductResponse>
    {
        public required string UserId { get; set; }
        public required string CategoryId { get; set; }
        public required string SubCategoryId { get; set; }
        public required string Title { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public string? CertificateId { get; set; }
        public DateTime? RegisteredSince { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }
        public string? LocationLongitude { get; set; }
        public string? LocationLatitude { get; set; }
        public bool Active { get; set; } = true;
        public ICollection<ProductImageRequest>? ProductImages { get; set; }
        public ICollection<ProductFacilityRequest>? ProductFacilities { get; set; }
        public ICollection<ProductNearRequest>? ProductNears { get; set; }
        public ICollection<ProductSpecificationRequest>? ProductSpecifications { get; set; }
    }
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand, AddProductResponse>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repoProduct;
        private readonly IBaseRepositoryAsync<ProductImage, string> _repoProductImage;
        readonly SlugHelper slugHelper;
        private readonly Random _random;

        public AddProductCommandHandler(IBaseRepositoryAsync<Product, string> repo,
            IBaseRepositoryAsync<ProductImage, string> repoProductImage)
        {
            _repoProduct = repo;
            slugHelper = new();
            _repoProductImage = repoProductImage;
            _random = new Random();
        }
        public async ValueTask<AddProductResponse> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            var product = MapToProduct(command);
            var resultProduct = await _repoProduct.AddAsync(product, cancellationToken);
            var response = MapToProductResponse(resultProduct);

            return new AddProductResponse
            {
                Success = true,
                Message = "Success Create Data",
                Data = response
            };
        }

        private static ProductItemResponse MapToProductResponse(Product resultProduct)
        {
            return new ProductItemResponse
            {
                Id = resultProduct.Id,
                UserId = resultProduct.UserId,
                CategoryId = resultProduct.CategoryId,
                CategoryName = resultProduct.Category?.Name,
                SubCategoryId = resultProduct.SubCategoryId,
                Title = resultProduct.Title,
                Slug = resultProduct.Slug,
                Province = resultProduct.Province,
                City = resultProduct.City,
                District = resultProduct.District,
                SubDistrict = resultProduct.SubDistrict,
                PostCode = resultProduct.PostCode,
                Address = resultProduct.Address,
                CertificateId = resultProduct.CertificateId,
                RegisteredSince = resultProduct.RegisteredSince,
                PriceFrom = resultProduct.PriceFrom,
                PriceTo = resultProduct.PriceTo,
                Description = resultProduct.Description,
                Details = resultProduct.Details,
                LocationLongitude = resultProduct.LocationLongitude,
                LocationLatitude = resultProduct.LocationLatitude,
                Active = resultProduct.Active,
                ProductImages = resultProduct.ProductImages?.Select(resultProductProductImage => new ProductImageItemResponse
                {
                    ProductId = resultProductProductImage.ProductId,
                    ImageUrl = resultProductProductImage.ImageUrl,
                    ImageType = resultProductProductImage.ImageType,
                    ImageName = resultProductProductImage.ImageName
                }).ToList(),
                ProductFacilities = resultProduct.ProductFacilities?.Select(resultProductProductFacility => new ProductFacilityResponse
                {
                    ProductId = resultProductProductFacility.ProductId,
                    FacilityId = resultProductProductFacility.FacilityId
                }).ToList(),
                ProductNears = resultProduct.ProductNears?.Select(resultProductProductNear => new ProductNearResponse
                {
                    ProductId = resultProductProductNear.ProductId,
                    Title = resultProductProductNear.Title,
                    ProductNearItems = resultProductProductNear.ProductNearItems?.Select(resultProductProductNearProductNearItem => new ProductNearItemResponse
                    {
                        ProductId = resultProductProductNearProductNearItem.ProductId,
                        ProductNearId = resultProductProductNearProductNearItem.ProductNearId,
                        Title = resultProductProductNearProductNearItem.Title
                    }).ToList()
                }).ToList(),
                ProductSpecifications = resultProduct.ProductSpecifications?.Select(resultProductProductSpecification => new ProductSpecificationResponse
                {
                    ProductId = resultProductProductSpecification.ProductId,
                    Title = resultProductProductSpecification.Title,
                    Description = resultProductProductSpecification.Description
                }).ToList()
            };
        }

        private Product MapToProduct(AddProductCommand command)
        {
            var slug = slugHelper.GenerateSlug($"{command.Title} {Guid.NewGuid().ToString("N")[..8]}");

            var prod = new Product
            {
                UserId = command.UserId,
                CategoryId = command.CategoryId,
                Title = command.Title,
                Slug = slug,
                SubCategoryId = command.SubCategoryId,
                CertificateId = command.CertificateId,
                Province = command.Province,
                City = command.City,
                District = command.District,
                SubDistrict = command.SubDistrict,
                PostCode = command.PostCode,
                Address = command.Address,
                RegisteredSince = command.RegisteredSince,
                PriceFrom = command.PriceFrom,
                PriceTo = command.PriceTo,
                Description = command.Description,
                Details = command.Details,
                LocationLongitude = command.LocationLongitude,
                LocationLatitude = command.LocationLatitude,
                Active = command.Active,
            };
            prod.ProductImages = command.ProductImages == null ? null : MapToProductImage(prod.Id, command.ProductImages);
            prod.ProductFacilities = command.ProductFacilities == null ? null : MapToProductFacilities(prod.Id, command.ProductFacilities);
            prod.ProductNears = command.ProductNears == null ? null : MapToProductNears(prod.Id, command.ProductNears);
            prod.ProductSpecifications = command.ProductSpecifications == null ? null : MapToProductSpesifications(prod.Id, command.ProductSpecifications);
            return prod;
        }

        private static ICollection<ProductSpecification>? MapToProductSpesifications(string productId, ICollection<ProductSpecificationRequest> productSpecifications)
        {
            return productSpecifications.Select(productSpecification => new ProductSpecification
            {
                ProductId = productId,
                Title = productSpecification.Title,
                Description = productSpecification.Description,
            }).ToList();
        }
        private static ICollection<ProductNear>? MapToProductNears(string productId, ICollection<ProductNearRequest> productNears)
        {
            return productNears.Select(productNear =>
            {
                var pn = new ProductNear
                {
                    ProductId = productId,
                    Title = productNear.Title,

                };
                pn.ProductNearItems = productNear.ProductNearItems == null ? null : MapToProductNearItems(productId, pn.Id, productNear.ProductNearItems);
                return pn;
            }
            ).ToList();
        }
        private static ICollection<ProductNearItem>? MapToProductNearItems(string productId, string productNearId, ICollection<ProductNearItemRequest> productNears)
        {
            return productNears.Select(productNear => new ProductNearItem
            {
                ProductId = productId,
                ProductNearId = productNearId,
                Title = productNear.Title
            }).ToList();
        }
        private static ICollection<ProductFacility>? MapToProductFacilities(string productid, ICollection<ProductFacilityRequest> productFacilities)
        {
            return productFacilities.Select(productFacility => new ProductFacility
            {
                ProductId = productid,
                FacilityId = productFacility.FacilityId
            }).ToList();
        }
        private static ICollection<ProductImage> MapToProductImage(string productid, ICollection<ProductImageRequest> enumerable)
        {
            return enumerable.Select(enumerableElement => new ProductImage
            {
                ProductId = productid,
                ImageUrl = enumerableElement.ImageUrl,
                ImageType = enumerableElement.ImageType,
                ImageName = enumerableElement.ImageName
            }).ToList();
        }
    }
}
