using Grpc.Core;
using GrpcIdentity;
using IdentityServer.Features.Users.Queries;

namespace IdentityServer.Grpc
{
    public class InMerchantService : InMerchantGrpc.InMerchantGrpcBase
    {
        private readonly IMediator _mediator;
        public InMerchantService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcInMerchantByCategoryResponse> GetItemsByCategorySlug(GrpcPagingByIdRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetMerchantByCategorySlugQuery() { CategoryId = request.Id, PageNumber = request.PageNumber, PageSize = request.PageSize });
            //var res = new GrpcInMerchantByCategoryResponse();
            var res = MaptoByCategoryResponse(items);
            res.Data.AddRange(items?.Data?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");
            return res;
        }

        private static GrpcInMerchantByCategoryResponse MaptoByCategoryResponse(BasePagingResponse<MerchantByCategoryItem> items)
        {
            return new GrpcInMerchantByCategoryResponse
            {
                TotalData = items.TotalData,
                Limit = items.Limit,
                CurrentPage = items.CurrentPage,
                Count = items.Count,
                Success = items.Success,
                Message = items.Message ?? ""
            };
        }

        private static GrpcInMerchantByCategoryItemResponse MapToItemResponse(MerchantByCategoryItem? c)
        {
            return c != null ? new GrpcInMerchantByCategoryItemResponse
            {
                Id = c.Id ?? "",
                CategoryId = c.CategoryId ?? "",
                BrandName = c.BrandName ?? "",
                BrandSlug = c.BrandSlug ?? "",
                Province = c.Province ?? "",
                City = c.City ?? "",
                District = c.District ?? "",
                Active = c.Active,
                ImageUrl = c.ImageUrl ?? "",
                IsSeller = c.IsSeller
            } : new GrpcInMerchantByCategoryItemResponse();
        }
    }
}
