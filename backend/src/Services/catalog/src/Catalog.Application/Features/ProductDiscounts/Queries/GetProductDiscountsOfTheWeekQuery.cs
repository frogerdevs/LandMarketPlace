using System.Globalization;

namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountsOfTheWeekQuery : IQuery<IEnumerable<ProductDiscountsOfTheWeekItem>?>
    {
    }
    public sealed class GetProductDiscountsOfTheWeekQueryHandler : IQueryHandler<GetProductDiscountsOfTheWeekQuery, IEnumerable<ProductDiscountsOfTheWeekItem>?>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public GetProductDiscountsOfTheWeekQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<ProductDiscountsOfTheWeekItem>?> Handle(GetProductDiscountsOfTheWeekQuery query, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DayOfWeek startOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime startOfThisWeek = now.Date.AddDays(-(int)now.DayOfWeek + (int)startOfWeek);
            DateTime endOfThisWeek = startOfThisWeek.AddDays(6);

            var items = await _repo.Entities.Include(c => c.Product).ThenInclude(c => c!.ProductImages).Include(c => c.Product!.Category)
                .AsNoTracking().Select(i => new ProductDiscountsOfTheWeekItem
                {
                    DiscountId = i.Id,
                    DiscountName = i.DiscountName,
                    DiscountStart = i.DiscountStart,
                    DiscountEnd = i.DiscountEnd,
                    DiscountPercent = i.DiscountPercent,
                    DiscountPrice = i.DiscountPrice,
                    Active = i.Active,
                    UserId = i.UserId,
                    ProductId = i.ProductId,
                    ProductSlug = i.Product!.Slug,
                    ProductTitle = i.Product.Title,
                    CategoryId = i.Product.CategoryId,
                    CategorySlug = i.Product.Category!.Slug,
                    City = i.Product.City,
                    Province = i.Product.Province,
                    District = i.Product.District,
                    PriceFrom = i.Product.PriceFrom,
                    PriceTo = i.Product.PriceTo,
                    ImageUrl = GetDefaultImage(i.Product.ProductImages),
                    ProductActive = i.Product.Active
                })
                .Where(c => c.DiscountStart.Date <= endOfThisWeek && c.DiscountEnd.Date >= startOfThisWeek && c.Active && c.ProductActive)
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
