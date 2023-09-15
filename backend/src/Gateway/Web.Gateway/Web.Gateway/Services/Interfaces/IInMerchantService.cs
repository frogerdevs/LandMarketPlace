using Web.Gateway.Dto.Response.InMerchants;

namespace Web.Gateway.Services.Interfaces
{
    public interface IInMerchantService
    {
        ValueTask<BasePagingResponse<MerchantByCategoryItemResponse>?> GetByCategorySlugAsync(BasePagingBySlugRequest request, CancellationToken cancellationToken = default);
    }
}
