namespace Catalog.Application.Features.Certifications.Queries
{
    public class GetCertificateByIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string Id { get; set; }
    }
    public sealed class GetCertificateByIdQueryHandler : IQueryHandler<GetCertificateByIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<CertificateType, string> _repo;

        public GetCertificateByIdQueryHandler(IBaseRepositoryAsync<CertificateType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetCertificateByIdQuery query, CancellationToken cancellationToken)
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
