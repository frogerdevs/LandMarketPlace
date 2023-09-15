namespace Ordering.Application.Features.BenefitCarts.Command
{
    public class DeleteBenefitCartCommand : IRequest<bool>
    {
        public required string Id { get; set; }
        public string? UpdateBy { get; set; }
    }
    public sealed class DeleteBenefitCartCommandhandler : IRequestHandler<DeleteBenefitCartCommand, bool>
    {
        private readonly IBaseRepositoryAsync<BenefitCart, string> _repo;

        public DeleteBenefitCartCommandhandler(IBaseRepositoryAsync<BenefitCart, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteBenefitCartCommand request, CancellationToken cancellationToken)
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
