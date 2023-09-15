namespace Catalog.Application.Features.Adsenses.Commands
{
    public class DeleteAdsenseCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public class DeleteAdsenseCommandHandler : IRequestHandler<DeleteAdsenseCommand, bool>
    {
        readonly IBaseRepositoryAsync<Adsense, string> _repository;
        public DeleteAdsenseCommandHandler(IBaseRepositoryAsync<Adsense, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<bool> Handle(DeleteAdsenseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) { return false; }

            entity.Active = false;
            await _repository.UpdateAsync(entity, request.Id, cancellationToken);
            return true;
        }
    }
}
