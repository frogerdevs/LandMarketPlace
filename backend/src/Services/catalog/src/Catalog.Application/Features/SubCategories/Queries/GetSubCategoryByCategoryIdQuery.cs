namespace Catalog.Application.Features.SubCategories.Queries
{
    public class GetSubCategoryByCategoryIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string CategoryId { get; set; }
    }
    public sealed class GetSubCategoryByCategoryIdQueryHandler : IQueryHandler<GetSubCategoryByCategoryIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _repo;

        public GetSubCategoryByCategoryIdQueryHandler(IBaseRepositoryAsync<SubCategory, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetSubCategoryByCategoryIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == query.CategoryId, cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }
}
