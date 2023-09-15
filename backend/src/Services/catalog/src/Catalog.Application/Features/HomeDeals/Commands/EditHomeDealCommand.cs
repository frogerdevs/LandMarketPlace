namespace Catalog.Application.Features.HomeDeals.Commands
{
    public class EditHomeDealCommand : ICommand<EditHomeDealResponse>
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class EditHomeDealCommandHandler : ICommandHandler<EditHomeDealCommand, EditHomeDealResponse>
    {
        readonly IBaseRepositoryAsync<HomeDeal, string> _repoHomeDeal;
        public EditHomeDealCommandHandler(IBaseRepositoryAsync<HomeDeal, string> repoHomeDeal)
        {
            _repoHomeDeal = repoHomeDeal;
        }
        public async ValueTask<EditHomeDealResponse> Handle(EditHomeDealCommand command, CancellationToken cancellationToken)
        {
            var homeDeal = await _repoHomeDeal.GetByIdAsync(command.Id, cancellationToken);
            if (homeDeal == null)
            {
                return null!;
            }
            homeDeal.ProductId = command.ProductId;
            homeDeal.ImgUrl = command.ImgUrl;
            homeDeal.StartDate = command.StartDate;
            homeDeal.EndDate = command.EndDate;
            homeDeal.Active = command.Active;

            var res = await _repoHomeDeal.UpdateAsync(homeDeal, homeDeal.Id, cancellationToken);
            var homeDealDto = MapToHomeDealDto(res);

            return new EditHomeDealResponse
            {
                Success = true,
                Message = "Success Edit Category",
                Data = homeDealDto
            };
        }

        private static HomeDealItemResponse MapToHomeDealDto(HomeDeal res)
        {
            return new HomeDealItemResponse
            {
                Id = res.Id,
                ProductId = res.ProductId,
                ImgUrl = res.ImgUrl,
                Active = res.Active,
                StartDate = res.StartDate,
                EndDate = res.EndDate,

            };
        }
    }
}
