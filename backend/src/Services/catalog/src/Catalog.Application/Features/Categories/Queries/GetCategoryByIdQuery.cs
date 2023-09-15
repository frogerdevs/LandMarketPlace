namespace Catalog.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IQuery<CategoryItemResponse?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryItemResponse?>
    {
        private readonly IBaseRepositoryAsync<Category, string> _repo;

        public GetCategoryByIdQueryHandler(IBaseRepositoryAsync<Category, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<CategoryItemResponse?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            return MapToResponse(items);
        }

        private static CategoryItemResponse? MapToResponse(Category? items)
        {
            return items!=null ? new CategoryItemResponse
            {
                Id=items.Id,
                Name=items.Name,
                Slug=items.Slug,
                Description=items.Description,
                ImageUrl=items.ImageUrl,
                Active=items.Active
            } : null;
        }
    }
}
