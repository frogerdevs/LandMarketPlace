namespace Catalog.Application.Features.ProductDiscounts.Command
{
    public partial class AddProductDiscountCommand : ICommand<BaseWithDataResponse>
    {
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public string? DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public bool Active { get; set; }
    }
    public class AddProductDiscountCommandHandler : ICommandHandler<AddProductDiscountCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;
        readonly SlugHelper slugHelper;

        public AddProductDiscountCommandHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
            slugHelper = new();
        }

        public async ValueTask<BaseWithDataResponse> Handle(AddProductDiscountCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug($"{command.DiscountName} {Guid.NewGuid().ToString("N")[..8]}");
            var productDiscount = MapToEntity(slug, command);
            var res = await _repo.AddAsync(productDiscount, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Create ProductDiscount",
                Data = res
            };
        }

        private static ProductDiscount MapToEntity(string slug, AddProductDiscountCommand command)
        {
            return new ProductDiscount
            {
                UserId = command.UserId,
                ProductId = command.ProductId,
                DiscountName = command.DiscountName,
                Slug = slug,
                DiscountPercent = command.DiscountPercent,
                DiscountPrice = command.DiscountPrice,
                DiscountStart = command.DiscountStart,
                DiscountEnd = command.DiscountEnd,
                Active = command.Active
            };
        }
    }

}
