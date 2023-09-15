namespace Catalog.Application.Features.HomeDeals.Queries
{
    public class GetDealsForHomeQuery : IQuery<IEnumerable<DealsForHomeItem>?>
    {
    }
    public sealed class GetDealsForHomeQueryHandler : IQueryHandler<GetDealsForHomeQuery, IEnumerable<DealsForHomeItem>?>
    {
        private readonly IBaseRepositoryAsync<HomeDeal, string> _repository;
        public GetDealsForHomeQueryHandler(IBaseRepositoryAsync<HomeDeal, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<IEnumerable<DealsForHomeItem>?> Handle(GetDealsForHomeQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.NoTrackingEntities
                .Where(c => c.Active)
                .Select(c => new DealsForHomeItem
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    Title = c.Product!.Title,
                    ImgUrl = GetDefaultImage(c.Product!.ProductImages),
                    ProductSlug = c.Product!.Slug,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Active = c.Active
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
