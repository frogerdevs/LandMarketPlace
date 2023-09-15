using Subscription.Application.Dtos.Response.Base;

namespace Subscription.Application.Dtos.Response.Brands
{
    public class BrandResponse : BaseResponse
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
    }
}
