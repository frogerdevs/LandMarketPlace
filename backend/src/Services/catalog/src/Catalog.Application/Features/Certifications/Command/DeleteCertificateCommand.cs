namespace Catalog.Application.Features.Certifications.Command
{
    public class DeleteCertificateCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public sealed class DeleteCertificateCommandhandler : IRequestHandler<DeleteCertificateCommand, bool>
    {
        private readonly IBaseRepositoryAsync<CertificateType, string> _repo;

        public DeleteCertificateCommandhandler(IBaseRepositoryAsync<CertificateType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the entity to be deleted from the database
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            // Remove the entity from the database
            await _repo.DeleteAsync(entity, cancellationToken);

            return true; // Return a completed Task
        }
    }
}
