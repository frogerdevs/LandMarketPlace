namespace Catalog.Application.Features.Facilities.Command
{
    public class DeleteFacilityCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public sealed class DeleteFacilityCommandhandler : IRequestHandler<DeleteFacilityCommand, bool>
    {
        private readonly IBaseRepositoryAsync<Facility, string> _repo;

        public DeleteFacilityCommandhandler(IBaseRepositoryAsync<Facility, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
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
