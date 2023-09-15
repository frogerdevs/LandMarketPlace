namespace Catalog.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        readonly IBaseRepositoryAsync<Product, string> _repository;
        public DeleteProductCommandHandler(IBaseRepositoryAsync<Product, string> repository)
        {
            _repository = repository;
        }
        public async ValueTask<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) { return false; }
            entity.Active = false;
            await _repository.UpdateAsync(entity, request.Id, cancellationToken);
            //await _repository.DeleteAsync(entity, cancellationToken);
            return true;
        }
    }
}
