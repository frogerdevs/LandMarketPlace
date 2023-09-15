namespace Catalog.Application.Features.Products.Commands
{
    public class EditProductCommand : ICommand<EditProductResponse>
    {
        public required string Id { get; set; }
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
        //public bool Active { get; set; }
        public ICollection<ProductImageRequest>? ProductImages { get; set; }
        public ICollection<ProductFacilityRequest>? ProductFacilities { get; set; }
        public ICollection<ProductNearRequest>? ProductNears { get; set; }
        public ICollection<ProductSpecificationRequest>? ProductSpecifications { get; set; }
    }
    public class EditProductCommandHandler : ICommandHandler<EditProductCommand, EditProductResponse>
    {
        readonly IBaseRepositoryAsync<Product, string> _repoProduct;
        readonly IBaseRepositoryAsync<ProductImage, string> _repoProductImage;
        private readonly Random _random;
        public EditProductCommandHandler(IBaseRepositoryAsync<Product, string> repoProduct,
            IBaseRepositoryAsync<ProductImage, string> repoProductImage)
        {
            _repoProduct = repoProduct;
            _repoProductImage = repoProductImage;
            _random = new Random();
        }
        public async ValueTask<EditProductResponse> Handle(EditProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _repoProduct.Entities.Include(c => c.ProductImages).Include(c => c.ProductFacilities)
                .Include(c => c.ProductNears).Include(c => c.ProductSpecifications)
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken: cancellationToken);

            if (product == null)
            {
                return null!;
            }

            if (product.ProductImages != null && product.ProductImages.Count > 0)
            {
                var deleteimages = await _repoProductImage.DeleteRangeAsync(product.ProductImages.ToList(), cancellationToken);
            }


            SlugHelper slugHelper = new();
            var slug = slugHelper.GenerateSlug($"{command.Title} {Guid.NewGuid().ToString("N")[..8]}");

            product.UserId = command.UserId;
            product.CategoryId = command.CategoryId;
            product.SubCategoryId = command.SubCategoryId;
            if (product.Title != command.Title)
            {
                product.Slug = slug;
                product.Title = command.Title;
            }
            product.Province = command.Province;
            product.City = command.City;
            product.District = command.District;
            product.SubDistrict = command.SubDistrict;
            product.PostCode = command.PostCode;
            product.Address = command.Address;
            product.CertificateId = command.CertificateId;
            product.RegisteredSince = command.RegisteredSince;
            product.PriceFrom = command.PriceFrom;
            product.PriceTo = command.PriceTo;
            product.Description = command.Description;
            product.Details = command.Details;
            product.LocationLongitude = command.LocationLongitude;
            product.LocationLatitude = command.LocationLatitude;
            //product.Active = command.Active;

            product.ProductImages?.Clear();
            product.ProductFacilities?.Clear();
            product.ProductNears?.Clear();
            product.ProductSpecifications?.Clear();
            product.ProductImages = command.ProductImages == null ? null : await MapToProductImage(product.Id, command.ProductImages);
            product.ProductFacilities = command.ProductFacilities == null ? null : await MapToProductFacilities(product.Id, command.ProductFacilities);
            product.ProductNears = command.ProductNears == null ? null : await MapToProductNears(product.Id, command.ProductNears);
            product.ProductSpecifications = command.ProductSpecifications == null ? null : await MapToProductSpesifications(product.Id, command.ProductSpecifications);

            var res = await _repoProduct.UpdateAsync(product, product.Id, cancellationToken);
            var productDto = MaptoProductDto(res);

            return new EditProductResponse
            {
                Success = true,
                Message = "Success Edit Product",
                Data = productDto
            };
        }

        private static Task<ICollection<ProductSpecification>> MapToProductSpesifications(string productid, ICollection<ProductSpecificationRequest> productSpecifications)
        {
            return Task.FromResult((ICollection<ProductSpecification>)productSpecifications.Select(productSpecification => new ProductSpecification
            {
                ProductId = productid,
                Title = productSpecification.Title,
                Description = productSpecification.Description
            }).ToList());
        }

        private static ProductItemResponse MaptoProductDto(Product res)
        {
            return new ProductItemResponse
            {
                Id = res.Id,
                UserId = res.UserId,
                CategoryId = res.CategoryId,
                CategoryName = res.Category?.Name,
                SubCategoryId = res.SubCategoryId,
                Title = res.Title,
                Slug = res.Slug,
                Province = res.Province,
                City = res.City,
                District = res.District,
                SubDistrict = res.SubDistrict,
                PostCode = res.PostCode,
                Address = res.Address,
                CertificateId = res.CertificateId,
                RegisteredSince = res.RegisteredSince,
                PriceFrom = res.PriceFrom,
                PriceTo = res.PriceTo,
                Description = res.Description,
                Details = res.Details,
                LocationLongitude = res.LocationLongitude,
                LocationLatitude = res.LocationLatitude,
                Active = res.Active,
                ProductImages = res.ProductImages?.Select(resProductImage => new ProductImageItemResponse
                {
                    ProductId = resProductImage.ProductId,
                    ImageUrl = resProductImage.ImageUrl,
                    ImageType = resProductImage.ImageType,
                    ImageName = resProductImage.ImageName
                }).ToList(),
                ProductFacilities = res.ProductFacilities?.Select(resProductFacility => new ProductFacilityResponse
                {
                    ProductId = resProductFacility.ProductId,
                    FacilityId = resProductFacility.FacilityId
                }).ToList(),
                ProductNears = res.ProductNears?.Select(resProductNear => new ProductNearResponse
                {
                    ProductId = resProductNear.ProductId,
                    Title = resProductNear.Title,
                    ProductNearItems = resProductNear.ProductNearItems?.Select(resProductNearProductNearItem => new ProductNearItemResponse
                    {
                        ProductId = resProductNearProductNearItem.ProductId,
                        ProductNearId = resProductNearProductNearItem.ProductNearId,
                        Title = resProductNearProductNearItem.Title
                    }).ToList()
                }).ToList(),
                ProductSpecifications = res.ProductSpecifications?.Select(resProductSpecification => new ProductSpecificationResponse
                {
                    ProductId = resProductSpecification.ProductId,
                    Title = resProductSpecification.Title,
                    Description = resProductSpecification.Description
                }).ToList()
            };
        }

        private static Task<ICollection<ProductImage>> MapToProductImage(string productid, ICollection<ProductImageRequest> enumerable)
        {
            ICollection<ProductImage> productImages = enumerable.Select(enumerableElement => new ProductImage
            {
                ProductId = productid,
                ImageUrl = enumerableElement.ImageUrl,
                ImageType = enumerableElement.ImageType,
                ImageName = enumerableElement.ImageName
            }).ToList();

            return Task.FromResult(productImages);
        }
        private static Task<ICollection<ProductNear>> MapToProductNears(string productId, ICollection<ProductNearRequest> productNears)
        {
            ICollection<ProductNear> nearFroms = productNears.Select((productNear) =>
            {
                var near = new ProductNear
                {
                    ProductId = productId,
                    Title = productNear.Title,
                };
                near.ProductNearItems = productNear.ProductNearItems == null ? null : MapToProductNearItems(productId, near.Id, productNear.ProductNearItems).ConfigureAwait(false).GetAwaiter().GetResult();
                return near;
            }
            ).ToList();
            return Task.FromResult(nearFroms);
        }
        private static Task<ICollection<ProductNearItem>> MapToProductNearItems(string productId, string productNearId, ICollection<ProductNearItemRequest> productNears)
        {
            ICollection<ProductNearItem> item = productNears.Select(productNear => new ProductNearItem
            {
                ProductId = productId,
                ProductNearId = productNearId,
                Title = productNear.Title
            }).ToList();
            return Task.FromResult(item);
        }
        private static Task<ICollection<ProductFacility>> MapToProductFacilities(string productid, ICollection<ProductFacilityRequest> productFacilities)
        {
            ICollection<ProductFacility> pc = productFacilities.Select(productFacility => new ProductFacility
            {
                ProductId = productid,
                FacilityId = productFacility.FacilityId
            }).ToList();
            return Task.FromResult(pc);
        }
    }
}
