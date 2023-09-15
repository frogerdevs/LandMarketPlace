namespace Catalog.Application.Features.Categories.Queries
{
    public class GetCategoryBySlugQuery : IQuery<CategoryItemResponse?>
    {
        public required string Slug { get; set; }
    }
    public sealed class GetCategoryBySlugQueryHandler : IQueryHandler<GetCategoryBySlugQuery, CategoryItemResponse?>
    {
        private readonly IBaseRepositoryAsync<Category, string> _repo;

        public GetCategoryBySlugQueryHandler(IBaseRepositoryAsync<Category, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<CategoryItemResponse?> Handle(GetCategoryBySlugQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities
                .Select(e => new CategoryItemResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Slug = e.Slug,
                    ImageUrl = e.ImageUrl,
                    Active = e.Active,
                    Description = e.Description,
                })
                .AsNoTracking().FirstOrDefaultAsync(c => c.Slug == query.Slug && c.Active, cancellationToken);

            return items;
        }

    }
}
