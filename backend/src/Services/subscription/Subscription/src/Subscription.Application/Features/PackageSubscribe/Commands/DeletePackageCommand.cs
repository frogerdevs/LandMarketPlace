namespace Subscription.Application.Features.PackageSubscribe.Commands
{
    public class DeletePackageCommand : IRequest<bool>
    {
        public required string Id { get; set; }
        public string? UpdateBy { get; set; }
    }
    public sealed class DeletePackageCommandhandler : IRequestHandler<DeletePackageCommand, bool>
    {
        private readonly IBaseRepositoryAsync<Package, string> _repo;

        public DeletePackageCommandhandler(IBaseRepositoryAsync<Package, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the entity to be deleted from the database
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            // Remove the entity from the database
            entity.Active = false;
            entity.UpdatedBy = request.UpdateBy;
            await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            //await _repo.DeleteAsync(entity, cancellationToken);

            return true; // Return a completed Task
        }
    }
}
