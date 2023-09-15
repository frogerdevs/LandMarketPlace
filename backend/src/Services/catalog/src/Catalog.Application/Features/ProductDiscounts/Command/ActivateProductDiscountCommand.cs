namespace Catalog.Application.Features.ProductDiscounts.Command
{
    public partial class ActivateProductDiscountCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public bool Active { get; set; }
    }
    public class ActivateProductDiscountCommandHandler : ICommandHandler<ActivateProductDiscountCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public ActivateProductDiscountCommandHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(ActivateProductDiscountCommand command, CancellationToken cancellationToken)
        {
            var productDiscount = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (productDiscount == null)
            {
                return null!;
            }
            productDiscount.Active = command.Active;
            var res = await _repo.UpdateAsync(productDiscount, productDiscount.Id, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Activate Product Discount",
                Data = res
            };
        }
    }

}
