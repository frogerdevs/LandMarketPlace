namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountsQuery : IQuery<IEnumerable<ProductDiscountItem>?>
    {
    }
    public sealed class GetProductDiscountsQueryHandler : IQueryHandler<GetProductDiscountsQuery, IEnumerable<ProductDiscountItem>?>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public GetProductDiscountsQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<ProductDiscountItem>?> Handle(GetProductDiscountsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities.Include(c => c.Product).ThenInclude(c => c!.ProductImages)
                .AsNoTracking().Select(i => new ProductDiscountItem
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    DiscountStart = i.DiscountStart,
                    DiscountEnd = i.DiscountEnd,
                    DiscountName = i.DiscountName,
                    DiscountPercent = i.DiscountPercent,
                    DiscountPrice = i.DiscountPrice,
                    ProductId = i.ProductId,
                    ProductTitle = i.Product!.Title,
                    Active = i.Active,
                    ImageUrl = GetDefaultImage(i.Product.ProductImages),
                }).ToListAsync(cancellationToken);

            return items;
        }

        private static string GetDefaultImage(ICollection<ProductImage>? productImages)
        {
            if (productImages != null)
            {
                var productImage = productImages.FirstOrDefault();
                if (productImage != null)
                    return productImage.ImageUrl ?? "";
            }
            return "";
        }
    }

}
