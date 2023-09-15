using GrpcCatalog;

namespace Web.Gateway.Services.Implementations
{
    public class CategoryService : ICategoryService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<CategoryService> _logger;
        readonly HttpClient _client;
        private readonly CategoryGrpc.CategoryGrpcClient _grpcClient;
        private readonly HomeGrpc.HomeGrpcClient _grpcClientHome;
        public CategoryService(IHttpClientFactory httpClientFactory,
            ILogger<CategoryService> logger, CategoryGrpc.CategoryGrpcClient grpcClient,
            HomeGrpc.HomeGrpcClient grpcClientHome)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
            _grpcClient = grpcClient;
            _grpcClientHome = grpcClientHome;
        }
        public async ValueTask<CategoriesResponse> GetForHomePageAsync()
        {
            var response = await _grpcClientHome.GetCategoriesAsync(new GrpcEmptyRequest());

            var res = MapGrpcHomeToListItem(response);
            //var url = UrlsConfig.CatalogService.CategoryForHome();
            //using HttpResponseMessage response = await _client.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //_logger.LogInformation("GetForHomePageAsync");

            //var result = await response.Content.ReadFromJsonAsync<CategoriesResponse>();

            return res;
        }

        private static CategoriesResponse MapGrpcHomeToListItem(GrpcHomeCategoryResponse response)
        {
            return new CategoriesResponse
            {
                Success = true,
                Message = "Success",
                Count = response.Data.Count,
                Data = response.Data.Select(responseDatum => new CategoryItem
                {
                    Id = responseDatum.Id,
                    Name = responseDatum.Name,
                    Slug = responseDatum.Slug,
                    Description = responseDatum.Description,
                    Active = responseDatum.Active
                })
            };
        }

        public async ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken)
        {
            var url = UrlsConfig.CatalogService.Category();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }
        public async ValueTask<BaseListResponse<CategoryItem>> GetGrpcAsync(CancellationToken cancellationToken)
        {
            GrpcCategoryResponse response = await _grpcClient.GetItemsAsync(new GrpcEmptyRequest { }, cancellationToken: cancellationToken);
            BaseListResponse<CategoryItem> result = MapGrpcCategoryToListItem(response);
            return result;
        }
        public async ValueTask<BaseListResponse<CategoryItem>> GetActiveGrpcAsync(CancellationToken cancellationToken)
        {
            GrpcCategoryResponse response = await _grpcClient.GetActiveItemsAsync(new GrpcActiveCategoryRequest { Active = true }, cancellationToken: cancellationToken);
            BaseListResponse<CategoryItem> result = MapGrpcCategoryToListItem(response);
            return result;
        }
        private static BaseListResponse<CategoryItem> MapGrpcCategoryToListItem(GrpcCategoryResponse response)
        {
            return new BaseListResponse<CategoryItem>
            {
                Success = true,
                Message = "Success",
                Data = response.Data.Select(responseDatum => new CategoryItem
                {
                    Id = responseDatum.Id,
                    Name = responseDatum.Name,
                    Slug = responseDatum.Slug,
                    Description = responseDatum.Description,
                    Active = responseDatum.Active
                })
            };
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.CatalogService.Category(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }
        public async ValueTask<CategoryItem?> GetGrpcByIdAsync(string id, CancellationToken cancellationToken)
        {
            var res = await _grpcClient.GetItemByIdAsync(new GrpcCatalog.GrpcByIdRequest { Id = id }, cancellationToken: cancellationToken);
            if (res == null)
                return null;

            return MaptoCategoryItem(res);
        }

        private static CategoryItem? MaptoCategoryItem(GrpcCategoryItemResponse? res)
        {
            return res != null ? new CategoryItem
            {
                Id = res.Id,
                Name = res.Name,
                Slug = res.Slug,
                Description = res.Description,
                Active = res.Active
            } : null;
        }

        public async ValueTask<CategoryItem?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var res = await _grpcClient.GetItemBySlugAsync(new GrpcBySlugRequest { Slug = slug }, cancellationToken: cancellationToken);
            if (res == null)
                return null;

            return MaptoCategoryItem(res);
        }

        public async ValueTask<HttpResponseMessage> PostAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.CatalogService.Category();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, CategoryPutRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.CatalogService.Category(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.CatalogService.Category(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }

    }
}
