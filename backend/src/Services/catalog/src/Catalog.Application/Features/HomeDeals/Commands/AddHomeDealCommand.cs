namespace Catalog.Application.Features.HomeDeals.Commands
{
    public class AddHomeDealCommand : ICommand<AddHomeDealResponse>
    {
        public required string ProductId { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class AddHomeDealCommandHandler : ICommandHandler<AddHomeDealCommand, AddHomeDealResponse>
    {
        private readonly IBaseRepositoryAsync<HomeDeal, string> _repoHomeDeal;
        private readonly IBaseRepositoryAsync<Product, string> _repoProduct;
        private readonly IBaseRepositoryAsync<ProductImage, string> _repoProductImage;

        public AddHomeDealCommandHandler(IBaseRepositoryAsync<HomeDeal, string> repoHomeDeal,
            IBaseRepositoryAsync<Product, string> repoProduct,
            IBaseRepositoryAsync<ProductImage, string> repoProductImage)
        {
            _repoHomeDeal = repoHomeDeal;
            _repoProduct = repoProduct;
            _repoProductImage = repoProductImage;

        }
        public async ValueTask<AddHomeDealResponse> Handle(AddHomeDealCommand command, CancellationToken cancellationToken)
        {
            var homeDeal = MapToHomeDeal(command);
            var resHomeDeal = await _repoHomeDeal.AddAsync(homeDeal, cancellationToken);
            var product = await _repoProduct.GetByIdAsync(command.ProductId, cancellationToken);
            var response = MapToHomeDealItemResponse(resHomeDeal, product);

            return new AddHomeDealResponse
            {
                Success = true,
                Message = "Success Create Data",
                Data = response
            };
        }

        private static HomeDealItemResponse MapToHomeDealItemResponse(HomeDeal homeDeal, Product? product)
        {
            return new HomeDealItemResponse
            {
                Id = homeDeal.Id,
                ProductId = homeDeal.ProductId,
                ImgUrl = homeDeal.ImgUrl,
                Active = homeDeal.Active,
                StartDate = homeDeal.StartDate,
                EndDate = homeDeal.EndDate,
                Product = product != null ? MapToProductItemResponse(product) : null
            };
        }

        private static HomeDeal MapToHomeDeal(AddHomeDealCommand command)
        {
            return new HomeDeal
            {
                ProductId = command.ProductId,
                ImgUrl = command.ImgUrl,
                Active = command.Active,
                StartDate = command.StartDate,
                EndDate = command.EndDate
            };
        }
        private static ProductItemResponse MapToProductItemResponse(Product product)
        {
            return new ProductItemResponse
            {
                Id = product.Id,
                UserId = product.UserId,
                CategoryId = product.CategoryId,
                Title = product.Title,
                Slug = product.Slug,
                PriceFrom = product.PriceFrom,
                PriceTo = product.PriceTo,
                Description = product.Description,
                Details = product.Details,
                LocationLongitude = product.LocationLongitude,
                LocationLatitude = product.LocationLatitude,
                Active = product.Active,
            };
        }
    }

}
