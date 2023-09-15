namespace Catalog.Application.Features.HomeDeals.Commands
{
    public partial class ActivateHomeDealCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public bool Active { get; set; }
    }
    public class ActivateHomeDealCommandHandler : ICommandHandler<ActivateHomeDealCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<HomeDeal, string> _repo;

        public ActivateHomeDealCommandHandler(IBaseRepositoryAsync<HomeDeal, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(ActivateHomeDealCommand command, CancellationToken cancellationToken)
        {
            var homeDeal = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (homeDeal == null)
            {
                return null!;
            }
            homeDeal.Active = command.Active;
            var res = await _repo.UpdateAsync(homeDeal, homeDeal.Id, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Activate In Deals",
                Data = res
            };
        }
    }

}
