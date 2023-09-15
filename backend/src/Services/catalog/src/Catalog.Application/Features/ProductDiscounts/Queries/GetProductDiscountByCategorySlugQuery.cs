namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountByCategorySlugQuery : IQuery<IEnumerable<ProductDiscountByCategoryItem>?>
    {
        public required string Slug { get; set; }
        public int PageSize { get; set; } = 16;
    }
    public sealed class GetProductDiscountByCategorySlugQueryHandler : IQueryHandler<GetProductDiscountByCategorySlugQuery, IEnumerable<ProductDiscountByCategoryItem>?>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public GetProductDiscountByCategorySlugQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<ProductDiscountByCategoryItem>?> Handle(GetProductDiscountByCategorySlugQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities.Include(c => c.Product).ThenInclude(c => c!.Category)
                .Select(item => new ProductDiscountByCategoryItem
                {
                    UserId = item.UserId,
                    CategoryId = item.Product!.CategoryId,
                    CategorySlug = item.Product.Category == null ? "" : item.Product.Category.Slug,
                    CategoryName = item.Product.Category!.Name,
                    DiscountId = item.Id,
                    DiscountName = item.DiscountName,
                    Slug = item.Slug,
                    DiscountPercent = item.DiscountPercent,
                    DiscountPrice = item.DiscountPrice,
                    DiscountStart = item.DiscountStart,
                    DiscountEnd = item.DiscountEnd,
                    Active = item.Active,

                    ProductId = item.ProductId,
                    ProductTitle = item.Product.Title ?? "",
                    ProductSlug = item.Product.Slug,
                    ProductActive = item.Product.Active,
                    Province = item.Product.Province,
                    City = item.Product.City,
                    District = item.Product.District,
                    PriceFrom = item.Product.PriceFrom,
                    PriceTo = item.Product.PriceTo,
                    ImageUrl = GetDefaultImage(item.Product.ProductImages), // item.Product.ProductImages!.FirstOrDefault()!.ImageUrl,
                })
                .AsNoTracking()
                .Take(query.PageSize)
                .Where(c => c.CategorySlug == query.Slug && c.Active && c.ProductActive)
                .OrderByDescending(c => c.DiscountStart)
                .ToListAsync(cancellationToken);

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
