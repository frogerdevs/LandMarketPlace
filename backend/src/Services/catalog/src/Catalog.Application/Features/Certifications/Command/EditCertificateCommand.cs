namespace Catalog.Application.Features.Certifications.Command
{
    public partial class EditCertificateCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool Active { get; set; }
    }
    public class EditCertificateCommandHandler : ICommandHandler<EditCertificateCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<CertificateType, string> _certificateTypeRepo;
        public EditCertificateCommandHandler(IBaseRepositoryAsync<CertificateType, string> certificateTypeRepo)
        {
            _certificateTypeRepo = certificateTypeRepo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(EditCertificateCommand command, CancellationToken cancellationToken)
        {
            var certificateType = await _certificateTypeRepo.GetByIdAsync(command.Id, cancellationToken);
            if (certificateType == null)
            {
                return null!;
            }
            certificateType.Code = command.Code;
            certificateType.Name = command.Name;
            certificateType.Active = command.Active;

            var res = await _certificateTypeRepo.UpdateAsync(certificateType, certificateType.Id, cancellationToken);

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Edit CertificateType",
                Data = res
            };
        }
    }
}
