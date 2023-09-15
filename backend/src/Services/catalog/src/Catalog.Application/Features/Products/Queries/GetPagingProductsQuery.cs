namespace Catalog.Application.Features.Products.Queries
{
    public class GetPagingProductsQuery : IQuery<BasePagingResponse<ProductsItem>>
    {
        public string? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? Search { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public bool? Active { get; set; }
    }
    public sealed class GetPagingProductsQueryHandler : IQueryHandler<GetPagingProductsQuery, BasePagingResponse<ProductsItem>>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repository;
        public GetPagingProductsQueryHandler(IBaseRepositoryAsync<Product, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<BasePagingResponse<ProductsItem>> Handle(GetPagingProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new BasePagingResponse<ProductsItem>
            {
                Limit = request.PageSize,
                CurrentPage = request.PageNumber
            };

            var queryRepo = _repository.Entities.Include(c => c.Category)
                            .Include(c => c.ProductImages).AsQueryable();

            if (request.UserId is not null)
            {
                queryRepo = queryRepo.Where(p => p.UserId == request.UserId);
            }
            if (!string.IsNullOrEmpty(request.ProductName) && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => p.Title.ToLower().Contains(request.ProductName.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Search) && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => (p.City != null && p.City.ToLower().Contains(request.Search.ToLower()))
                || (p.District != null && p.District.ToLower().Contains(request.Search.ToLower()))
                || (p.Title.ToLower().Contains(request.Search.ToLower())));
            }
            if (request.CategoryName is not null && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => p.Category == null || p.Category.Name.ToLower().Contains(request.CategoryName.ToLower()));
            }
            if (request.Price > decimal.Zero && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => p.PriceFrom <= request.Price && p.PriceTo >= request.Price);
            }
            if (request.Active is not null && await queryRepo.AnyAsync(cancellationToken: cancellationToken))
            {
                queryRepo = queryRepo.Where(p => p.Active == request.Active);
            }

            response.TotalData = await queryRepo.CountAsync(cancellationToken: cancellationToken);
            var items = await queryRepo
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
                            })
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);

            response.Count = items.Count;
            response.Data = items;
            response.Success = true;
            response.Message = "Success Get Data";

            return response;
        }
    }

}
