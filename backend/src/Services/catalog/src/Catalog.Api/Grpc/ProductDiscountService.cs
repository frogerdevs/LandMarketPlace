using Google.Protobuf.WellKnownTypes;

namespace Catalog.Api.Grpc
{
    public class ProductDiscountService : ProductDiscountGrpc.ProductDiscountGrpcBase
    {
        private readonly IMediator _mediator;
        public ProductDiscountService(IMediator mediator)
        {

            _mediator = mediator;

        }
        public override async Task<GrpcProductDiscountByCategorySlugResponse> GetItemsByCategorySlug(GrpcByCategorySlugRequest request, ServerCallContext context)
        {
            var query = new GetProductDiscountByCategorySlugQuery
            {
                Slug = request.Slug,
                PageSize = request.PageSize
            };
            var items = await _mediator.Send(query);

            GrpcProductDiscountByCategorySlugResponse res = new();
            res.Data.AddRange(MapToByCategoryList(items));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcProductDiscountItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProductDiscountByIdQuery() { Id = request.Id });

            var res = MapToItem(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }


        public override async Task<GrpcProductDiscountResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetProductDiscountsQuery());

            GrpcProductDiscountResponse res = new();
            res.Data.AddRange(MapToList(items));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }


        public override async Task<GrpcProductDiscountsOfTheWeekResponse> GetItemsOfTheWeek(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetProductDiscountsOfTheWeekQuery());
            GrpcProductDiscountsOfTheWeekResponse res = new();
            res.Data.AddRange(MapToListOfTheWeek(items));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }


        #region MyRegion
        private static IEnumerable<GrpcProductDiscountOfTheWeekItem> MapToListOfTheWeek(IEnumerable<ProductDiscountsOfTheWeekItem>? items)
        {
            return items?.Select(item => new GrpcProductDiscountOfTheWeekItem
            {
                UserId = item.UserId,
                CategoryId = item.CategoryId ?? throw new NullReferenceException("The value of 'item.CategoryId' should not be null"),
                CategorySlug = item.CategorySlug ?? throw new NullReferenceException("The value of 'item.CategorySlug' should not be null"),
                DiscountId = item.DiscountId ?? throw new NullReferenceException("The value of 'item.DiscountId' should not be null"),
                DiscountName = item.DiscountName ?? throw new NullReferenceException("The value of 'item.DiscountName' should not be null"),
                DiscountPercent = (double)item.DiscountPercent,
                DiscountPrice = (double)item.DiscountPrice,
                DiscountStart = Timestamp.FromDateTimeOffset(item.DiscountStart),
                DiscountEnd = Timestamp.FromDateTimeOffset(item.DiscountEnd),
                ProductId = item.ProductId,
                ProductTitle = item.ProductTitle,
                ProductSlug = item.ProductSlug,
                ProductActive = item.ProductActive,
                Province = item.Province ?? throw new NullReferenceException("The value of 'item.Province' should not be null"),
                City = item.City ?? throw new NullReferenceException("The value of 'item.City' should not be null"),
                District = item.District ?? throw new NullReferenceException("The value of 'item.District' should not be null"),
                PriceFrom = (double)item.PriceFrom,
                PriceTo = (double)item.PriceTo,
                ImageUrl = item.ImageUrl ?? throw new NullReferenceException("The value of 'item.ImageUrl' should not be null")
            }) ?? throw new NullReferenceException("The value of 'items' should not be null");
        }

        private static IEnumerable<GrpcProductDiscountItemResponse> MapToList(IEnumerable<ProductDiscountItem>? items)
        {
            return items?.Select(item => new GrpcProductDiscountItemResponse
            {
                Id = item.Id,
                UserId = item.UserId,
                ProductId = item.ProductId,
                ProductTitle = item.ProductTitle,
                DiscountName = item.DiscountName ?? throw new NullReferenceException("The value of 'item.DiscountName' should not be null"),
                DiscountPercent = (double)item.DiscountPercent,
                DiscountPrice = (double)item.DiscountPrice,
                DiscountStart = Timestamp.FromDateTimeOffset(item.DiscountStart),
                DiscountEnd = Timestamp.FromDateTimeOffset(item.DiscountEnd),
                ImageUrl = item.ImageUrl ?? throw new NullReferenceException("The value of 'item.ImageUrl' should not be null"),
                Active = item.Active
            }) ?? throw new NullReferenceException("The value of 'items' should not be null");
        }

        private static IEnumerable<GrpcProductDiscountByCategorySlugItem>? MapToByCategoryList(IEnumerable<ProductDiscountByCategoryItem>? items)
        {
            return items?.Select(item => new GrpcProductDiscountByCategorySlugItem
            {
                UserId = item.UserId,
                CategoryId = item.CategoryId,
                CategorySlug = item.CategorySlug,
                CategoryName = item.CategoryName,
                DiscountId = item.DiscountId,
                DiscountName = item.DiscountName,
                Slug = item.Slug,
                DiscountPercent = (double)item.DiscountPercent,
                DiscountPrice = (double)item.DiscountPrice,
                DiscountStart = Timestamp.FromDateTimeOffset(item.DiscountStart),
                DiscountEnd = Timestamp.FromDateTimeOffset(item.DiscountEnd),
                Active = item.Active,
                ProductId = item.ProductId,
                ProductTitle = item.ProductTitle,
                ProductSlug = item.ProductSlug,
                Province = item.Province,
                City = item.City,
                District = item.District,
                PriceFrom = (double)item.PriceFrom,
                PriceTo = (double)item.PriceTo,
                ImageUrl = item.ImageUrl,
            });
        }

        private GrpcProductDiscountItemResponse MapToItem(ProductDiscountItem? item)
        {
            return item != null ? new GrpcProductDiscountItemResponse
            {
                Id = item.Id,
                UserId = item.UserId,
                ProductId = item.ProductId,
                ProductTitle = item.ProductTitle,
                DiscountName = item.DiscountName ?? throw new NullReferenceException("The value of 'item.DiscountName' should not be null"),
                DiscountPercent = (double)item.DiscountPercent,
                DiscountPrice = (double)item.DiscountPrice,
                DiscountStart = Timestamp.FromDateTimeOffset(item.DiscountStart),
                DiscountEnd = Timestamp.FromDateTimeOffset(item.DiscountEnd),
                ImageUrl = item.ImageUrl ?? throw new NullReferenceException("The value of 'item.ImageUrl' should not be null"),
                Active = item.Active
            } : throw new NullReferenceException("The value of 'item' should not be null");
        }
        #endregion
    }
}
