namespace Catalog.Application.Features.Products.Queries
{
    public class GetProductsQuery : IQuery<IEnumerable<ProductsItem>?>
    {
    }
    public sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, IEnumerable<ProductsItem>?>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repository;
        public GetProductsQueryHandler(IBaseRepositoryAsync<Product, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<IEnumerable<ProductsItem>?> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.Entities.Include(c => c.Category).Include(c => c.ProductImages)
                .Include(c => c.ProductFacilities).Include(c => c.ProductNears)
                .Include(c => c.ProductSpecifications)
                .AsNoTracking()
                            .Select(c => new ProductsItem
                            {
                                Id = c.Id,
                                CategoryId = c.CategoryId,
                                CategoryName = c.Category == null ? "" : c.Category.Name,
                                CategorySlug = c.Category == null ? "" : c.Category.Slug,
                                Title = c.Title,
                                Slug = c.Slug,
                                Address = c.Address,
                                PriceFrom = c.PriceFrom,
                                PriceTo = c.PriceTo,
                                ImageUrl = c.ProductImages == null ? "" : (c.ProductImages.FirstOrDefault() == null ? "" : c.ProductImages.FirstOrDefault()!.ImageUrl),
                                Active = c.Active,
                                UserId = c.UserId,
                                City = c.City,
                                District = c.District,
                            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
