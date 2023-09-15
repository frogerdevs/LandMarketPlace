namespace Catalog.Api.Grpc
{
    public class CategoryService : CategoryGrpc.CategoryGrpcBase
    {
        readonly IMediator _mediator;
        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GrpcCategoryResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetCategoriesQuery());
            var res = new GrpcCategoryResponse();

            res.Data.AddRange(items.Select(c => MapToRes(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        public override async Task<GrpcCategoryResponse> GetActiveItems(GrpcActiveCategoryRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetCategoriesQuery() { Active = request.Active });
            var res = new GrpcCategoryResponse();

            res.Data.AddRange(items.Select(c => MapToRes(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcCategoryItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetCategoryByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data dengan Id :{request.Id} tidak ditemukan");
                return new GrpcCategoryItemResponse();
            }
            var res = MapToRes(item);
            context.Status = new Status(StatusCode.OK, $" Success");
            return res;
        }
        public override async Task<GrpcCategoryItemResponse> GetItemBySlug(GrpcBySlugRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetCategoryBySlugQuery() { Slug = request.Slug });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcCategoryItemResponse();
            }
            var res = MapToRes(item);
            context.Status = new Status(StatusCode.OK, $" Success");
            return res;
        }
        private static GrpcCategoryItemResponse MapToRes(CategoryItemResponse item)
        {
            return new GrpcCategoryItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Slug = item.Slug,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Active = item.Active
            };
        }
    }
}
