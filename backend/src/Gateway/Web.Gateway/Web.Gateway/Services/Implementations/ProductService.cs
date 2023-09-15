using Google.Protobuf.Collections;
using GrpcCatalog;
using GrpcIdentity;

namespace Web.Gateway.Services.Implementations
{
    public class ProductService : IProductService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProductService> _logger;
        readonly HttpClient _client;
        private readonly ProductGrpc.ProductGrpcClient _productGrpcClient;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly RegionAddressGrpc.RegionAddressGrpcClient _regionAddressGrpcClient;
        public ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger,
            ProductGrpc.ProductGrpcClient grpcClientProduct, UserGrpc.UserGrpcClient userGrpcClient,
            RegionAddressGrpc.RegionAddressGrpcClient regionAddressGrpcClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
            _productGrpcClient = grpcClientProduct;
            _userGrpcClient = userGrpcClient;
            _regionAddressGrpcClient = regionAddressGrpcClient;
        }

        public async ValueTask<IEnumerable<ProductsItem>?> GetAsync(CancellationToken cancellationToken = default)
        {
            var response = await _productGrpcClient.GetItemsAsync(new GrpcCatalog.GrpcEmptyRequest { }, cancellationToken: cancellationToken);
            return MapToList(response.Data);
        }


        public async ValueTask<BasePagingResponse<ProductsByCategoryItemResponse>> GetPagingAsync(PagingProductRequest request, CancellationToken cancellationToken = default)
        {
            string? query = QueryStringBuilderExtension<PagingProductRequest>.BuildQueryString(request);

            var productGrpcResponse = await _productGrpcClient.GetPagingItemsAsync(new GrpcCatalog.GrpcPagingProductRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                UserId = request.UserId,
                CategoryName = request.CategoryName,
                ProductName = request.ProductName,
                Price = Convert.ToDouble(request.Price),
                Active = request.Active ?? false,
            }, cancellationToken: cancellationToken);
            if (productGrpcResponse != null)
            {
                var result = new BasePagingResponse<ProductsByCategoryItemResponse>
                {
                    Success = true,
                    Message = "Success Get Data",
                    TotalData = productGrpcResponse.TotalData,
                    Count = productGrpcResponse.Count,
                    CurrentPage = productGrpcResponse.CurrentPage,
                    Limit = productGrpcResponse.Limit,
                    Data = await MaptoProductsPaging(productGrpcResponse.Data)
                };
                //BasePagingResponse<ProductsByCategoryItemResponse> result = await MapToPagingIndeals(response, cancellationToken);

                return result;
            }

            return new BasePagingResponse<ProductsByCategoryItemResponse>
            {
                Success = true,
                Message = "Success Get Data",
                TotalData = 0,
                Count = 0,
                CurrentPage = request.PageSize,
                Limit = request.PageSize,
                Data = Array.Empty<ProductsByCategoryItemResponse>()
            };
            //var url = UrlsConfig.CatalogService.ProductPaging(query);
            //HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            //return response;
        }

        private async Task<IEnumerable<ProductsByCategoryItemResponse>> MaptoProductsPaging(RepeatedField<GrpcProductsItem> data)
        {
            var mappedResults = await Task.WhenAll(data.Select(async datum =>
            {
                var brandslug = await _userGrpcClient.GetUserBrandAsync(new GrpcIdentity.GrpcByIdRequest { Id = datum.UserId });
                return new ProductsByCategoryItemResponse
                {
                    Id = datum.Id,
                    UserId = datum.UserId,
                    CategoryId = datum.CategoryId,
                    CategoryName = datum.CategoryName,
                    CategorySlug = datum.CategorySlug,
                    Title = datum.Title,
                    Slug = datum.Slug,
                    BrandSlug = brandslug.BrandSlug,
                    //Province = datu
                    City = datum.City,
                    District = datum.District,
                    PriceFrom = (decimal)datum.PriceFrom,
                    PriceTo = (decimal)datum.PriceTo,
                    Active = datum.Active,
                    ImageUrl = datum.ImageUrl,
                    UrlSlug = $"{datum.CategorySlug}/{brandslug.BrandSlug}/{datum.Slug}"
                };
            }));

            return mappedResults;
        }

        public async ValueTask<ProductItemResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _productGrpcClient.GetItemByIdAsync(new GrpcCatalog.GrpcByIdRequest { Id = id }, cancellationToken: cancellationToken);
            var result = MaptoProductItem(response);
            return result;
        }

        public async ValueTask<ProductItemResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var productGrpcResponse = await _productGrpcClient.GetItemBySlugAsync(new GrpcCatalog.GrpcBySlugRequest { Slug = slug }, cancellationToken: cancellationToken);
            var result = MaptoProductItem(productGrpcResponse);
            if (result != null)
            {
                var contactResponse = await _userGrpcClient.GetContactPersonAsync(new GrpcIdentity.GrpcByIdRequest { Id = productGrpcResponse.UserId }, cancellationToken: cancellationToken);

                result.ProductContactPerson = MapToContactPerson(contactResponse);
                result.ProductUserSubscription = new ProductUserSubscription() { SubscriptionType = "Superior" };
            }

            return result;
        }

        private static ProductContactPerson? MapToContactPerson(GrpcContactPersonResponse? contactResponse)
        {
            return contactResponse != null ? new ProductContactPerson
            {
                UserId = contactResponse.UserId,
                BrandName = contactResponse.BrandName,
                BrandSlug = contactResponse.BrandSlug,
                ImageUrl = contactResponse.ImageUrl,
                Email = contactResponse.Email,
                Contact = contactResponse.Contact,
                WhatsApp = contactResponse.WhatsApp,
                Facebook = contactResponse.Facebook,
                Instagram = contactResponse.Instagram,
                Twitter = contactResponse.Twitter,
                Tiktok = contactResponse.Tiktok,
                Website = contactResponse.Website
            } : null;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(ProductRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Product();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, ProductPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Product(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Product(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<BasePagingResponse<ProductsByCategoryItemResponse>?> GetByCategorySlugAsync(PagingBySlugRequest request, CancellationToken cancellationToken = default)
        {
            var productGrpcResponse = await _productGrpcClient.GetItemsByCategorySlugAsync(new GrpcCatalog.GrpcPagingBySlugRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Slug = request.Slug
            }, cancellationToken: cancellationToken);
            if (productGrpcResponse != null)
            {
                var result = new BasePagingResponse<ProductsByCategoryItemResponse>
                {
                    Success = productGrpcResponse.Success,
                    Message = productGrpcResponse.Message,
                    TotalData = productGrpcResponse.TotalData,
                    Count = productGrpcResponse.Count,
                    CurrentPage = productGrpcResponse.CurrentPage,
                    Limit = productGrpcResponse.Limit,
                    Data = await MaptoProductsByCategories(productGrpcResponse.Data)
                };

                return result;
            }

            return new BasePagingResponse<ProductsByCategoryItemResponse>
            {
                Success = true,
                Message = "Success Get Data",
                TotalData = 0,
                Count = 0,
                CurrentPage = request.PageNumber,
                Limit = request.PageSize,
            };
        }

        private async Task<IEnumerable<ProductsByCategoryItemResponse>> MaptoProductsByCategories(RepeatedField<GrpcProductsByCategoryItemResponse> data)
        {
            var mappedResults = await Task.WhenAll(data.Select(async datum =>
            {
                var brandslug = await _userGrpcClient.GetUserBrandAsync(new GrpcIdentity.GrpcByIdRequest { Id = datum.UserId });
                return new ProductsByCategoryItemResponse
                {
                    Id = datum.Id,
                    UserId = datum.UserId,
                    CategoryId = datum.CategoryId,
                    CategoryName = datum.CategoryName,
                    CategorySlug = datum.CategorySlug,
                    Title = datum.Title,
                    Slug = datum.Slug,
                    Province = datum.Province,
                    City = datum.City,
                    District = datum.District,
                    PriceFrom = (decimal)datum.PriceFrom,
                    PriceTo = (decimal)datum.PriceTo,
                    Active = datum.Active,
                    ImageUrl = datum.ImageUrl,
                    BrandSlug = brandslug.BrandSlug,
                    UrlSlug = $"{datum.CategorySlug}/{brandslug.BrandSlug}/{datum.Slug}"
                };
            }));

            return mappedResults;
        }

        public async ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductByUser(userid);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        #region Method
        private static IEnumerable<ProductsItem>? MapToList(RepeatedField<GrpcProductsItem> data)
        {
            return data.Select(datum => new ProductsItem
            {
                Id = datum.Id,
                UserId = datum.UserId,
                CategoryId = datum.CategoryId,
                CategoryName = datum.CategoryName,
                CategorySlug = datum.CategorySlug,
                Title = datum.Title,
                Slug = datum.Slug,
                City = datum.City,
                District = datum.District,
                Address = datum.Address,
                PriceFrom = (decimal)datum.PriceFrom,
                PriceTo = (decimal)datum.PriceTo,
                ImageUrl = datum.ImageUrl,
                Active = datum.Active
            });
        }

        private static ProductItemResponse? MaptoProductItem(GrpcProductItemResponse? response)
        {
            return response != null ? new ProductItemResponse
            {
                Id = response.Id,
                UserId = response.UserId,
                CategoryId = response.CategoryId,
                CategoryName = response.CategoryName,
                SubCategoryId = response.SubCategoryId,
                SubCategoryName = response.SubCategoryName,
                Title = response.Title,
                Slug = response.Slug,
                Province = response.Province,
                City = response.City,
                District = response.District,
                SubDistrict = response.SubDistrict,
                PostCode = response.PostCode,
                Address = response.Address,
                CertificateId = response.CertificateId,
                RegisteredSince = response.RegisteredSince.ToDateTime(),
                PriceFrom = (decimal)response.PriceFrom,
                PriceTo = (decimal)response.PriceTo,
                Description = response.Description,
                Details = response.Details,
                LocationLongitude = response.LocationLongitude,
                LocationLatitude = response.LocationLatitude,
                Active = response.Active,
                ProductImages = response.ProductImages.Select(responseProductImage => new ProductImageItemResponse
                {
                    ProductId = responseProductImage.ProductId,
                    ImageUrl = responseProductImage.ImageUrl,
                    ImageType = responseProductImage.ImageType,
                    ImageName = responseProductImage.ImageName
                }).ToList(),
                ProductFacilities = response.ProductFacilities.Select(responseProductFacility => new ProductFacilityResponse
                {
                    ProductId = responseProductFacility.ProductId,
                    FacilityId = responseProductFacility.FacilityId,
                    FacilityName = responseProductFacility.FacilityName
                }).ToList(),
                ProductNears = response.ProductNears.Select(responseProductNear => new ProductNearResponse
                {
                    ProductId = responseProductNear.ProductId,
                    Title = responseProductNear.Title,
                    ProductNearItems = responseProductNear.ProductNearItems.Select(responseProductNearProductNearItem => new ProductNearItemResponse
                    {
                        ProductId = responseProductNearProductNearItem.ProductId,
                        ProductNearId = responseProductNearProductNearItem.ProductNearId,
                        Title = responseProductNearProductNearItem.Title
                    }).ToList()
                }).ToList(),
                ProductSpecifications = response.ProductSpecifications.Select(responseProductSpecification => new ProductSpecificationResponse
                {
                    ProductId = responseProductSpecification.ProductId,
                    Title = responseProductSpecification.Title,
                    Description = responseProductSpecification.Description
                }).ToList(),

            } : null;
        }

        #endregion
    }
}
