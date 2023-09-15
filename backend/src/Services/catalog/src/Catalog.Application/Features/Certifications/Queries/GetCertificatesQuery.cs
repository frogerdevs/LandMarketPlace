namespace Catalog.Application.Features.Certifications.Queries
{
    public class GetCertificatesQuery : IQuery<BaseWithDataCountResponse>
    {
    }
    public sealed class GetCertificatesQueryHandler : IQueryHandler<GetCertificatesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<CertificateType, string> _repo;

        public GetCertificatesQueryHandler(IBaseRepositoryAsync<CertificateType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetCertificatesQuery query, CancellationToken cancellationToken)
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
