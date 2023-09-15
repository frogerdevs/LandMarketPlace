namespace Catalog.Application.Features.Categories.Queries
{
    public class GetCategoriesQuery : IQuery<IEnumerable<CategoryItemResponse>>
    {
        public bool? Active { get; set; }
    }
    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryItemResponse>>
    {
        private readonly IBaseRepositoryAsync<Category, string> _repo;

        public GetCategoriesQueryHandler(IBaseRepositoryAsync<Category, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<CategoryItemResponse>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities.AsNoTracking().Select(i => new CategoryItemResponse
            {
                Id = i.Id,
                Name = i.Name!,
                Slug = i.Slug,
                ImageUrl = i.ImageUrl,
                Active = i.Active,
                Description = i.Description,
            }).Where(c => query.Active == null || c.Active == query.Active).ToListAsync(cancellationToken);

            return items;
        }

    }

}
