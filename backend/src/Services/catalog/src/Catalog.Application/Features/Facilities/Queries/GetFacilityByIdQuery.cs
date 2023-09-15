namespace Catalog.Application.Features.Facilities.Queries
{
    public class GetFacilityByIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string Id { get; set; }
    }
    public sealed class GetFacilityByIdQueryHandler : IQueryHandler<GetFacilityByIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Facility, string> _repo;

        public GetFacilityByIdQueryHandler(IBaseRepositoryAsync<Facility, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetFacilityByIdQuery query, CancellationToken cancellationToken)
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
