namespace Catalog.Application.Features.ProductDiscounts.Command
{
    public partial class EditProductDiscountCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public string? DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public bool Active { get; set; }
    }
    public class EditProductDiscountCommandHandler : ICommandHandler<EditProductDiscountCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public EditProductDiscountCommandHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(EditProductDiscountCommand command, CancellationToken cancellationToken)
        {
            SlugHelper slugHelper = new();
            var slug = slugHelper.GenerateSlug($"{command.DiscountName} {Guid.NewGuid().ToString("N")[..8]}");
            var productDiscount = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (productDiscount == null)
            {
                return null!;
            }
            productDiscount.ProductId = command.ProductId;
            productDiscount.DiscountName = command.DiscountName;
            productDiscount.Slug = slug;
            productDiscount.DiscountPercent = command.DiscountPercent;
            productDiscount.DiscountPrice = command.DiscountPrice;
            productDiscount.DiscountStart = command.DiscountStart;
            productDiscount.DiscountEnd = command.DiscountEnd;
            productDiscount.Active = command.Active;
            var res = await _repo.UpdateAsync(productDiscount, productDiscount.Id, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Update Product Discount",
                Data = res
            };
        }
    }

}
