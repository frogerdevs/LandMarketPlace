namespace Catalog.Application.Features.Categories.Queries
{
    public class GetCategoriesForHomeQuery : IQuery<IEnumerable<CategoriesForHomeItemResponse>?>
    {
    }
    public sealed class GetCategoriesForHomeQueryHandler : IQueryHandler<GetCategoriesForHomeQuery, IEnumerable<CategoriesForHomeItemResponse>?>
    {
        private readonly IBaseRepositoryAsync<Category, string> _repository;
        public GetCategoriesForHomeQueryHandler(IBaseRepositoryAsync<Category, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<IEnumerable<CategoriesForHomeItemResponse>?> Handle(GetCategoriesForHomeQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.Entities.AsNoTracking()
                .Where(c => c.Active)
                .Select(c => new CategoriesForHomeItemResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Active = c.Active,
                    Slug = c.Slug,
                }).ToListAsync(cancellationToken);

            //var res = new BaseWithDataCountResponse
            //{
            //    Success = true,
            //    Message = "Success Get Data",
            //    Count = items.Count,
            //    Data = items
            //};
            return items;
        }
    }

}
