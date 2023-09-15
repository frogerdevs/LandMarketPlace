namespace Subscription.Application.Features.UnitItems.Commands
{
    public class DeleteUnitItemCommand : IRequest<bool>
    {
        public required string Id { get; set; }
        public string? UpdateBy { get; set; }
    }
    public sealed class DeleteUnitItemCommandhandler : IRequestHandler<DeleteUnitItemCommand, bool>
    {
        private readonly IBaseRepositoryAsync<UnitItem, string> _repo;

        public DeleteUnitItemCommandhandler(IBaseRepositoryAsync<UnitItem, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteUnitItemCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the entity to be deleted from the database
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            // Remove the entity from the database
            entity.Active = false;
            entity.DeleteBy = request.UpdateBy;
            await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            //await _repo.DeleteAsync(entity, cancellationToken);

            return true; // Return a completed Task
        }
    }
}
