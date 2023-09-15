namespace Catalog.Application.Features.HomeDeals.Commands
{
    public class DeleteHomeDealCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public class DeleteHomeDealCommandHandler : IRequestHandler<DeleteHomeDealCommand, bool>
    {
        readonly IBaseRepositoryAsync<HomeDeal, string> _repository;
        public DeleteHomeDealCommandHandler(IBaseRepositoryAsync<HomeDeal, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<bool> Handle(DeleteHomeDealCommand request, CancellationToken cancellationToken)
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
