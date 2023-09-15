namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountByIdQuery : IQuery<ProductDiscountItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetProductDiscountByIdQueryHandler : IQueryHandler<GetProductDiscountByIdQuery, ProductDiscountItem?>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;
        private readonly IBaseRepositoryAsync<ProductImage, string> _repoProductImage;

        public GetProductDiscountByIdQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo, IBaseRepositoryAsync<ProductImage, string> repoProductImage)
        {
            _repo = repo;
            _repoProductImage = repoProductImage;
        }

        public async ValueTask<ProductDiscountItem?> Handle(GetProductDiscountByIdQuery query, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(query.Id, cancellationToken);
            var res = MapToItem(item);
            return res;
        }

        private ProductDiscountItem? MapToItem(ProductDiscount? item)
        {
            return item != null ? new ProductDiscountItem
            {
                Id = item.Id,
                UserId = item.UserId,
                ProductId = item.ProductId,
                ProductTitle = item.Product?.Title ?? throw new NullReferenceException("The value of 'item.Product?.Title' should not be null"),
                DiscountName = item.DiscountName,
                DiscountPercent = item.DiscountPercent,
                DiscountPrice = item.DiscountPrice,
                DiscountStart = item.DiscountStart,
                DiscountEnd = item.DiscountEnd,
                Active = item.Active,
                ImageUrl = GetDefaultImage(item.ProductId),
            } : null;
        }
        private string GetDefaultImage(string productId)
        {
            var productImage = _repoProductImage.NoTrackingEntities.FirstOrDefault(c => c.ProductId == productId);

            if (productImage != null)
                return productImage.ImageUrl ?? "";

            return "";
        }

    }

}
