namespace Catalog.Application.Features.Certifications.Command
{
    public partial class AddCertificateCommand : ICommand<BaseWithDataResponse>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
    public class AddCertificateCommandHandler : ICommandHandler<AddCertificateCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<CertificateType, string> _certificateTypeRepo;
        readonly SlugHelper slugHelper;

        public AddCertificateCommandHandler(IBaseRepositoryAsync<CertificateType, string> certificateTypeRepo)
        {
            slugHelper = new();
            _certificateTypeRepo = certificateTypeRepo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(AddCertificateCommand command, CancellationToken cancellationToken)
        {
            var certificate = MapToCertificate(command);
            var res = await _certificateTypeRepo.AddAsync(certificate, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Create CertificateType",
                Data = res
            };
        }

        private static CertificateType MapToCertificate(AddCertificateCommand command)
        {
            return new CertificateType
            {
                Code = command.Code,
                Name = command.Name,
                Active = command.Active,
            };
        }
    }

}
