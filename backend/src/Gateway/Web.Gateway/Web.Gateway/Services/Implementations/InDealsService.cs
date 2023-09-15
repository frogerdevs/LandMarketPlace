using GrpcCatalog;
using GrpcIdentity;

namespace Web.Gateway.Services.Implementations
{
    public class InDealsService : IInDealsService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<InDealsService> _logger;
        readonly HttpClient _client;
        private readonly ProductGrpc.ProductGrpcClient _productGrpcClient;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly RegionAddressGrpc.RegionAddressGrpcClient _regionAddressGrpcClient;
        readonly HomeGrpc.HomeGrpcClient _homeGrpcClient;

        public InDealsService(IHttpClientFactory httpClientFactory, ILogger<InDealsService> logger,
            ProductGrpc.ProductGrpcClient grpcClientProduct, UserGrpc.UserGrpcClient userGrpcClient,
            RegionAddressGrpc.RegionAddressGrpcClient regionAddressGrpcClient, HomeGrpc.HomeGrpcClient homeGrpcClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
            _productGrpcClient = grpcClientProduct;
            _userGrpcClient = userGrpcClient;
            _regionAddressGrpcClient = regionAddressGrpcClient;
            _homeGrpcClient = homeGrpcClient;
        }

        public async ValueTask<HomeDealsResponse> GetForHomePageAsync(CancellationToken cancellationToken = default)
        {
            var response = await _homeGrpcClient.GetInDealsAsync(new GrpcCatalog.GrpcEmptyRequest { }, cancellationToken: cancellationToken);
            HomeDealsResponse result = await MapToHomeDeals(response);

            return result;
        }

        private static Task<HomeDealsResponse> MapToHomeDeals(GrpcHomeInDealsResponse response)
        {
            return Task.FromResult(new HomeDealsResponse
            {
                Success = true,
                Message = "Success",
                Count = response.Data.Count,
                Data = response.Data.Select(responseDatum => new HomeDealsItem
                {
                    Id = responseDatum.Id,
                    Slug = responseDatum.Slug,
                    ImageUrl = responseDatum.ImageUrl,
                    Title = responseDatum.Title,
                    StartDate = responseDatum.StartDate.ToDateTime(),
                    EndDate = responseDatum.EndDate.ToDateTime(),
                    Active = responseDatum.Active
                })
            });
        }


        //public async Task<HomeDealsResponse> GetDealsForHomeAsync()
        //{
        //    _logger.LogInformation("GetDealsForHomeAsync");
        //    return await Task.Run(() =>
        //    {
        //        var dt = new List<HomeDealsItem>
        //        {
        //            new HomeDealsItem { Id = 1, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 1" },
        //            new HomeDealsItem { Id = 2, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 2"},
        //            new HomeDealsItem { Id = 3, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 3" },
        //            new HomeDealsItem { Id = 4, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 4" },
        //            new HomeDealsItem { Id = 5, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 5" },
        //            new HomeDealsItem { Id = 6, ImgUrl = "a471887cf10449c8b33dadcf130b65f5.png", Title = "Citra Garden 6" },
        //        };

        //        var res = new HomeDealsResponse()
        //        {
        //            Success = true,
        //            Message = string.Empty,
        //            Data = dt
        //        };

        //        Thread.Sleep(10);

        //        return res;
        //    });
        //}

        public ValueTask<HomeDealsResponse> GetDealsItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<BasePagingResponse<InDealsItem>> GetInDealsAsync(InDealsRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _productGrpcClient.GetIndealsAsync(new GrpcPagingIndealsRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Search = request.Search
            }, cancellationToken: cancellationToken);
            BasePagingResponse<InDealsItem> result = await MapToPagingIndeals(response, cancellationToken);
            return result;
        }

        private async ValueTask<BasePagingResponse<InDealsItem>> MapToPagingIndeals(GrpcPagingProductResponse response, CancellationToken cancellation)
        {
            return new BasePagingResponse<InDealsItem>
            {
                Count = response.Count,
                TotalData = response.TotalData,
                Limit = response.Limit,
                CurrentPage = response.CurrentPage,
                Data = await Task.WhenAll(response.Data.Select(async responseDatum => await MapToDealsItem(responseDatum, cancellation)))
            };
        }

        private async ValueTask<InDealsItem> MapToDealsItem(GrpcProductsItem responseDatum, CancellationToken cancellation)
        {
            var user = await _userGrpcClient.GetUserBrandAsync(new GrpcIdentity.GrpcByIdRequest() { Id = responseDatum.UserId }, cancellationToken: cancellation);
            var city = await _regionAddressGrpcClient.GetCityByIdAsync(new GrpcIdentity.GrpcByIdRequest() { Id = responseDatum.City }, cancellationToken: cancellation);
            var district = await _regionAddressGrpcClient.GetDisctrictByIdAsync(new GrpcIdentity.GrpcByIdRequest() { Id = responseDatum.District }, cancellationToken: cancellation);
            var merchantSubscription = "L";
            return new InDealsItem
            {
                Slug = responseDatum.Slug,
                Title = responseDatum.Title,
                PriceFrom = (decimal)responseDatum.PriceFrom,
                PriceTo = (decimal)responseDatum.PriceTo,
                City = city?.Name,
                District = district?.Name,
                ImageUrl = responseDatum.ImageUrl,
                MerchantName = user?.BrandName,
                MerchantSlug = user?.BrandSlug,
                MerchantSubscription = merchantSubscription,

            };
        }

    }
}
