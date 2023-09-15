using Google.Protobuf.WellKnownTypes;

namespace Catalog.Api.Grpc
{
    public class HomeService : HomeGrpc.HomeGrpcBase
    {
        readonly IMediator _mediator;
        public HomeService(IMediator mediator)
        {

            _mediator = mediator;

        }
        public override async Task<GrpcHomeCategoryResponse> GetCategories(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetCategoriesForHomeQuery());

            var res = new GrpcHomeCategoryResponse();
            res.Data.AddRange(MapToCategoryResponse(items));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        private static IEnumerable<GrpcHomeCategoryItemResponse>? MapToCategoryResponse(IEnumerable<CategoriesForHomeItemResponse>? items)
        {
            return items?.Select(item => new GrpcHomeCategoryItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Slug = item.Slug,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Active = item.Active
            });
        }

        public override async Task<GrpcHomeInDealsResponse> GetInDeals(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetDealsForHomeQuery());
            var res = new GrpcHomeInDealsResponse();
            res.Data.AddRange(MapToIndealResponse(items));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        private static IEnumerable<GrpcHomeInDealsItemResponse>? MapToIndealResponse(IEnumerable<DealsForHomeItem>? items)
        {
            return items?.Select(item => new GrpcHomeInDealsItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Title = item.Title,
                ImageUrl = item.ImgUrl,
                Slug = item.ProductSlug,
                StartDate = Timestamp.FromDateTimeOffset(item.StartDate),
                EndDate = Timestamp.FromDateTimeOffset(item.EndDate),
                Active = item.Active
            });
        }
    }
}
