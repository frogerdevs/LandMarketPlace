namespace Catalog.Application.Features.SubCategories.Queries
{
    public class GetSubCategoryByIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string Id { get; set; }
    }
    public sealed class GetSubCategoryByIdQueryHandler : IQueryHandler<GetSubCategoryByIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _repo;

        public GetSubCategoryByIdQueryHandler(IBaseRepositoryAsync<SubCategory, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetSubCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }
}
