namespace Catalog.Application.Features.Adsenses.Commands
{
    public class AddAdsenseCommand : ICommand<BaseWithDataResponse?>
    {
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
    public class AddAdsenseCommandHandler : ICommandHandler<AddAdsenseCommand, BaseWithDataResponse?>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repoAdsense;
        readonly SlugHelper slugHelper;

        public AddAdsenseCommandHandler(IBaseRepositoryAsync<Adsense, string> repoAdsense)
        {
            _repoAdsense = repoAdsense;
            slugHelper = new();
        }
        public async ValueTask<BaseWithDataResponse?> Handle(AddAdsenseCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug($"{command.Title} {Guid.NewGuid().ToString("N")[..8]}");
            var entity = MapToAdsense(slug, command);
            var res = await _repoAdsense.AddAsync(entity, cancellationToken);

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Create Data",
                Data = res
            };
        }

        private static Adsense MapToAdsense(string slug, AddAdsenseCommand command)
        {
            return new Adsense
            {
                ProductId = command.ProductId,
                UserId = command.UserId,
                ImageUrl = command.ImageUrl,
                Title = command.Title,
                Slug = slug,
                Content = command.Content,
                StartFrom = command.StartFrom,
                StartTo = command.StartTo,
                Active = command.Active,
                Channel = command.Channel
            };
        }
    }
}
