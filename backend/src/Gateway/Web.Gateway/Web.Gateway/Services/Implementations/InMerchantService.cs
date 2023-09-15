using Google.Protobuf.Collections;
using GrpcCatalog;
using GrpcIdentity;

namespace Web.Gateway.Services.Implementations
{
    public class InMerchantService : IInMerchantService, IScopedDependency
    {
        private readonly CategoryGrpc.CategoryGrpcClient _grpcClient;
        readonly InMerchantGrpc.InMerchantGrpcClient _inMerchantGrpcClient;
        public InMerchantService(InMerchantGrpc.InMerchantGrpcClient inMerchantGrpcClient,
            CategoryGrpc.CategoryGrpcClient grpcClient)
        {
            _inMerchantGrpcClient = inMerchantGrpcClient;
            _grpcClient = grpcClient;
        }
        public async ValueTask<BasePagingResponse<MerchantByCategoryItemResponse>?> GetByCategorySlugAsync(BasePagingBySlugRequest request, CancellationToken cancellationToken = default)
        {
            var res = await _grpcClient.GetItemBySlugAsync(new GrpcCatalog.GrpcBySlugRequest { Slug = request.Slug }, cancellationToken: cancellationToken);
            if (res != null)
            {
                var productGrpcResponse = await _inMerchantGrpcClient.GetItemsByCategorySlugAsync(new GrpcPagingByIdRequest
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Id = res.Id
                }, cancellationToken: cancellationToken);
                if (productGrpcResponse != null)
                {
                    var result = new BasePagingResponse<MerchantByCategoryItemResponse>
                    {
                        Success = productGrpcResponse.Success,
                        Message = productGrpcResponse.Message,
                        TotalData = productGrpcResponse.TotalData,
                        Count = productGrpcResponse.Count,
                        CurrentPage = productGrpcResponse.CurrentPage,
                        Limit = productGrpcResponse.Limit,
                        Data = await MaptoMerchantByCategories(res.Name, res.Slug, productGrpcResponse.Data)
                    };

                    return result;
                }
            }
            return new BasePagingResponse<MerchantByCategoryItemResponse>
            {
                Success = true,
                Message = "Success Get Data",
                TotalData = 0,
                Count = 0,
                CurrentPage = request.PageNumber,
                Limit = request.PageSize,
            };
        }

        private static Task<IEnumerable<MerchantByCategoryItemResponse>> MaptoMerchantByCategories(string name, string categoryslug, RepeatedField<GrpcInMerchantByCategoryItemResponse> data)
        {
            return Task.FromResult(data.Select(datum => new MerchantByCategoryItemResponse
            {
                Id = datum.Id,
                CategoryId = datum.CategoryId,
                BrandName = datum.BrandName,
                BrandSlug = datum.BrandSlug,
                Province = datum.Province,
                City = datum.City,
                District = datum.District,
                Active = datum.Active,
                ImageUrl = datum.ImageUrl,
                IsSeller = datum.IsSeller,
                CategoryName = name,
                CategorySlug = categoryslug,
                UrlSlug = $"{categoryslug}/{datum.BrandSlug}"
            }));
        }
    }
}
