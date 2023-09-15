namespace Catalog.Application.Features.Categories.Queries
{
    public class GetActiveCategoriesQuery : IQuery<IEnumerable<CategoryItemResponse>>
    {
    }
    public sealed class GetActiveCategoriesQueryHandler : IQueryHandler<GetActiveCategoriesQuery, IEnumerable<CategoryItemResponse>>
    {
        private readonly IBaseRepositoryAsync<Category, string> _repo;

        public GetActiveCategoriesQueryHandler(IBaseRepositoryAsync<Category, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<CategoryItemResponse>> Handle(GetActiveCategoriesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities.AsNoTracking().Select(i => new CategoryItemResponse
            {
                Id = i.Id,
                Name = i.Name!,
                Slug = i.Slug,
                ImageUrl = i.ImageUrl,
                Active = i.Active,
                Description = i.Description,
            }).Where(c => c.Active == true)
                .ToListAsync(cancellationToken);

            return items;
        }

    }
}
