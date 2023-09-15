using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.SubCategories.Queries
{
    public class GetSubCategoriesQuery : IQuery<BaseWithDataCountResponse>
    {
    }
    public sealed class GetSubCategoriesQueryHandler : IQueryHandler<GetSubCategoriesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _repo;

        public GetSubCategoriesQueryHandler(IBaseRepositoryAsync<SubCategory, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetSubCategoriesQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.AsNoTracking().Select(i => new
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                Name = i.Name!,
                Slug = i.Slug,
                ImageUrl = i.ImageUrl,
                Active = i.Active,
                Description = i.Description,
            }).ToListAsync(cancellationToken);
            res.Count = items.Count;
            res.Data = items;

            return res;
        }

    }
}
