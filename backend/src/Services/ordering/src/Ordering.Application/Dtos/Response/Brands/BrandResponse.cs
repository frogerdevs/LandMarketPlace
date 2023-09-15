using Ordering.Application.Dtos.Response.Base;

namespace Ordering.Application.Dtos.Respons.Brands
{
    public class BrandResponse : BaseResponse
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
    }
}
