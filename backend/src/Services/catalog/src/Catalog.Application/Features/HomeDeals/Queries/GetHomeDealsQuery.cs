namespace Catalog.Application.Features.HomeDeals.Queries
{
    public class GetHomeDealsQuery : IQuery<IEnumerable<HomeDealsListItem>>
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string? Search { get; set; }

    }
    public sealed class GetHomeDealsQueryHandler : IQueryHandler<GetHomeDealsQuery, IEnumerable<HomeDealsListItem>>
    {
        private readonly IBaseRepositoryAsync<HomeDeal, string> _repository;
        public GetHomeDealsQueryHandler(IBaseRepositoryAsync<HomeDeal, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<IEnumerable<HomeDealsListItem>> Handle(GetHomeDealsQuery query, CancellationToken cancellationToken)
        {
            var queryRepo = _repository.NoTrackingEntities.Include(c => c.Product).ThenInclude(c => c!.ProductImages)
                .Select(c => new HomeDealsListItem
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product!.Title,
                    ImageUrl = !string.IsNullOrEmpty(c.ImgUrl) ? c.ImgUrl : GetDefaultImage(c.Product.ProductImages),
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Active = c.Active
                });

            if (!string.IsNullOrEmpty(query.Search) && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => (p.ProductName != null && p.ProductName.ToLower().Contains(query.Search.ToLower())));
            }
            if (query.PageNumber > 0 && query.PageSize > 0)
            {
                queryRepo = queryRepo.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

            }

            var items = await queryRepo.ToListAsync(cancellationToken);


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
