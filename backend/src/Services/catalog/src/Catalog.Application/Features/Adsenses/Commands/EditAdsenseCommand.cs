namespace Catalog.Application.Features.Adsenses.Commands
{
    public partial class EditAdsenseCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public required string UserId { get; set; }
        public string? ImageUrl { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime StartTo { get; set; }
        public bool Active { get; set; }
        public string Channel { get; set; } = "unknown";
    }
    public class EditAdsenseCommandHandler : ICommandHandler<EditAdsenseCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repo;
        readonly SlugHelper slugHelper;
        public EditAdsenseCommandHandler(IBaseRepositoryAsync<Adsense, string> repo)
        {
            _repo = repo;
            slugHelper = new();
        }

        public async ValueTask<BaseWithDataResponse> Handle(EditAdsenseCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug($"{command.Title} {Guid.NewGuid().ToString("N")[..8]}");
            var adsense = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (adsense == null)
            {
                return null!;
            }
            adsense.ProductId = command.ProductId;
            adsense.ProductId = command.ProductId;
            adsense.UserId = command.UserId;
            adsense.ImageUrl = command.ImageUrl;
            adsense.Title = command.Title;
            adsense.Slug = slug;
            adsense.Content = command.Content;
            adsense.StartFrom = command.StartFrom;
            adsense.StartTo = command.StartTo;
            adsense.Active = command.Active;
            adsense.Channel = command.Channel;
            var res = await _repo.UpdateAsync(adsense, adsense.Id, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Update Adsense",
                Data = res
            };
        }
    }
}
