namespace Catalog.Application.Features.Facilities.Queries
{
    public class GetFacilitiesQuery : IQuery<BaseWithDataCountResponse>
    {
    }
    public sealed class GetFacilitiesQueryHandler : IQueryHandler<GetFacilitiesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<Facility, string> _repo;

        public GetFacilitiesQueryHandler(IBaseRepositoryAsync<Facility, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetFacilitiesQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.AsNoTracking().Select(i => new
            {
                Id = i.Id,
                Code = i.Code,
                Name = i.Name!,
                Active = i.Active,
            }).ToListAsync(cancellationToken);
            res.Count = items.Count;
            res.Data = items;

            return res;
        }
    }
}
