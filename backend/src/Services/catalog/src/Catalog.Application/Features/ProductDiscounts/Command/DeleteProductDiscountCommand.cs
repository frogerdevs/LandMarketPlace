namespace Catalog.Application.Features.ProductDiscounts.Command
{
    public class DeleteProductDiscountCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public sealed class DeleteProductDiscountCommandhandler : IRequestHandler<DeleteProductDiscountCommand, bool>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public DeleteProductDiscountCommandhandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteProductDiscountCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the entity to be deleted from the database
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            // Remove the entity from the database
            entity.Active = false;
            //await _repo.UpdateAsync(entity, request.Id, cancellationToken);
            await _repo.DeleteAsync(entity, cancellationToken);

            return true; // Return a completed Task
        }
    }
}
