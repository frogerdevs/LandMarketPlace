using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.SubCategories.Queries
{
    public class GetSubCategoryBySlugQuery : IQuery<BaseWithDataResponse>
    {
        public required string Slug { get; set; }
    }
    public sealed class GetSubCategoryBySlugQueryHandler : IQueryHandler<GetSubCategoryBySlugQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _repo;

        public GetSubCategoryBySlugQueryHandler(IBaseRepositoryAsync<SubCategory, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetSubCategoryBySlugQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities
                .Select(e => new
                {
                    e.Name,
                    e.Slug,
                    e.ImageUrl,
                    e.Active,
                    e.Description,
                })
                .AsNoTracking().FirstOrDefaultAsync(c => c.Slug == query.Slug && c.Active, cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = new
            {
                items.Name,
                items.Slug,
                items.Description,
                items.ImageUrl
            };

            return res;
        }

    }
}
