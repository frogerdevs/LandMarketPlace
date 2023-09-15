namespace Subscription.Application.Features.UnitTypes.Commands
{
    public class DeleteUnitTypeCommand : IRequest<bool>
    {
        public required string Id { get; set; }
        public string? UpdateBy { get; set; }
    }
    public sealed class DeleteUnitTypeCommandhandler : IRequestHandler<DeleteUnitTypeCommand, bool>
    {
        private readonly IBaseRepositoryAsync<BenefitType, string> _repo;

        public DeleteUnitTypeCommandhandler(IBaseRepositoryAsync<BenefitType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteUnitTypeCommand request, CancellationToken cancellationToken)
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
