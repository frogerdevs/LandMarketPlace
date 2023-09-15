using Google.Protobuf.Collections;
using GrpcCatalog;

namespace Web.Gateway.Services.Implementations
{
    public class ProductDiscountService : IProductDiscountService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProductDiscountService> _logger;
        readonly HttpClient _client;
        readonly ProductDiscountGrpc.ProductDiscountGrpcClient _grpcClient;
        public ProductDiscountService(IHttpClientFactory httpClientFactory, ILogger<ProductDiscountService> logger,
            ProductDiscountGrpc.ProductDiscountGrpcClient grpcClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
            _grpcClient = grpcClient;
        }

        public async ValueTask<IEnumerable<ProductDiscountItem>> GetAsync(CancellationToken cancellationToken = default)
        {
            var response = await _grpcClient.GetItemsAsync(new GrpcEmptyRequest { }, cancellationToken: cancellationToken);
            return MapToList(response.Data);
        }

        public async ValueTask<ProductDiscountItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _grpcClient.GetItemByIdAsync(new GrpcByIdRequest { Id = id }, cancellationToken: cancellationToken);
            var result = MapToItem(response);
            return result;
        }

        public async ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscountBySlug(slug);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscountByUser(userid);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(ProductDiscountRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscount();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, ProductDiscountPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscount(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscount(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> ActivateAsync(string id, ActivateProductDiscountRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.ProductDiscountActivate(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<IEnumerable<ProductDiscountByCategorySlugItem>?> GetByCategorySlugAsync(string categoryslug, CancellationToken cancellationToken = default)
        {
            var response = await _grpcClient.GetItemsByCategorySlugAsync(new GrpcByCategorySlugRequest
            {
                Slug = categoryslug,
                PageSize = 16
            }, cancellationToken: cancellationToken);
            return MapToByCategoryResponse(response?.Data);
        }
        public async ValueTask<IEnumerable<ProductDiscountsOfTheWeekItem>?> GetOfTheweekAsync(CancellationToken cancellationToken = default)
        {
            var response = await _grpcClient.GetItemsOfTheWeekAsync(new GrpcEmptyRequest(), cancellationToken: cancellationToken);
            return MapToOfTheWeekResponse(response.Data);
        }

        #region Method

        private static IEnumerable<ProductDiscountItem> MapToList(RepeatedField<GrpcProductDiscountItemResponse> data)
        {
            return data.Select(datum => new ProductDiscountItem
            {
                Id = datum.Id,
                UserId = datum.UserId,
                ProductId = datum.ProductId,
                ProductTitle = datum.ProductTitle,
                DiscountName = datum.DiscountName,
                DiscountPercent = (decimal)datum.DiscountPercent,
                DiscountPrice = (decimal)datum.DiscountPrice,
                DiscountStart = datum.DiscountStart.ToDateTime(),
                DiscountEnd = datum.DiscountEnd.ToDateTime(),
                Active = datum.Active,
                ImageUrl = datum.ImageUrl,

            });
        }
        private static ProductDiscountItem MapToItem(GrpcProductDiscountItemResponse response)
        {
            return new ProductDiscountItem
            {
                Id = response.Id,
                UserId = response.UserId,
                ProductId = response.ProductId,
                ProductTitle = response.ProductTitle,
                DiscountName = response.DiscountName,
                DiscountPercent = (decimal)response.DiscountPercent,
                DiscountPrice = (decimal)response.DiscountPrice,
                DiscountStart = response.DiscountStart.ToDateTime(),
                DiscountEnd = response.DiscountEnd.ToDateTime(),
                Active = response.Active,
                ImageUrl = response.ImageUrl
            };
        }
        private static IEnumerable<ProductDiscountByCategorySlugItem>? MapToByCategoryResponse(RepeatedField<GrpcProductDiscountByCategorySlugItem>? data)
        {
            return data?.Select(datum => new ProductDiscountByCategorySlugItem
            {
                UserId = datum.UserId,
                CategoryId = datum.CategoryId,
                CategorySlug = datum.CategorySlug,
                CategoryName = datum.CategoryName,
                DiscountId = datum.DiscountId,
                DiscountName = datum.DiscountName,
                Slug = datum.Slug,
                DiscountPercent = (decimal)datum.DiscountPercent,
                DiscountPrice = (decimal)datum.DiscountPrice,
                DiscountStart = datum.DiscountStart.ToDateTime(),
                DiscountEnd = datum.DiscountEnd.ToDateTime(),
                Active = datum.Active,
                ProductId = datum.ProductId,
                ProductTitle = datum.ProductTitle,
                ProductSlug = datum.ProductSlug,
                Province = datum.Province,
                City = datum.City,
                District = datum.District,
                PriceFrom = (decimal)datum.PriceFrom,
                PriceTo = (decimal)datum.PriceTo,
                ImageUrl = datum.ImageUrl
            });
        }
        private static IEnumerable<ProductDiscountsOfTheWeekItem>? MapToOfTheWeekResponse(RepeatedField<GrpcProductDiscountOfTheWeekItem> data)
        {
            return data.Select(datum => new ProductDiscountsOfTheWeekItem
            {
                UserId = datum.UserId,
                CategoryId = datum.CategoryId,
                CategorySlug = datum.CategorySlug,
                DiscountId = datum.DiscountId,
                DiscountName = datum.DiscountName,
                DiscountPercent = (decimal)datum.DiscountPercent,
                DiscountPrice = (decimal)datum.DiscountPrice,
                DiscountStart = datum.DiscountStart.ToDateTime(),
                DiscountEnd = datum.DiscountEnd.ToDateTime(),
                ProductId = datum.ProductId,
                ProductTitle = datum.ProductTitle,
                ProductSlug = datum.ProductSlug,
                ProductActive = datum.ProductActive,
                Province = datum.Province,
                City = datum.City,
                District = datum.District,
                PriceFrom = (decimal)datum.PriceFrom,
                PriceTo = (decimal)datum.PriceTo,
                ImageUrl = datum.ImageUrl
            });
        }

        #endregion

    }
}
