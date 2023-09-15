using Grpc.Core;
using GrpcSubscription;

namespace Subscription.Api.Grpc
{
    public class UnitItemService : UnitItemGrpc.UnitItemGrpcBase
    {
        readonly IMediator _mediator;
        public UnitItemService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcUnitItemsResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetUnitItemsQuery());
            var res = new GrpcUnitItemsResponse();
            res.Data.AddRange(items?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");
            return res;
        }

        public override async Task<GrpcUnitItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetUnitItemByIdQuery() { Id = request.Id, });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcUnitItemResponse();
            }
            var res = MapToItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");
            return res;
        }
        public override async Task<GrpcUnitItemResponse> AddItem(GrpcAddUnitItemRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new AddUnitItemCommand()
            {
                Title = request.Title,
                Description = request.Description,
                ValidDuration = request.ValidDuration,
                LiveDuration = request.LiveDuration,
                Priority = request.Priority,
                ShowInPackage = request.ShowInPackage,
                ShowInPageInDealPrice = request.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = request.ShowInPageInSpirationPrice,
                BenefitType = request.BenefitType,
                BenefitSize = request.BenefitSize,
                QuantityUpload = request.QuantityUpload,
                Active = request.Active,
                Price = (decimal)request.Price,
                DiscountPrice = (decimal)request.DiscountPrice,
                DiscountPercent = request.DiscountPercent,
                CreatedBy = request.CreatedBy,
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcUnitItemResponse();
        }
        public override async Task<GrpcUnitItemResponse> EditItem(GrpcEditUnitItemRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new EditUnitItemCommand()
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                ValidDuration = request.ValidDuration,
                LiveDuration = request.LiveDuration,
                Priority = request.Priority,
                ShowInPackage = request.ShowInPackage,
                ShowInPageInDealPrice = request.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = request.ShowInPageInSpirationPrice,
                BenefitType = request.BenefitType,
                BenefitSize = request.BenefitSize,
                QuantityUpload = request.QuantityUpload,
                Active = request.Active,
                Price = (decimal)request.Price,
                DiscountPrice = (decimal)request.DiscountPrice,
                DiscountPercent = request.DiscountPercent,
                UpdateBy = request.UpdateBy
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcUnitItemResponse();
        }
        public override async Task<GrpcBoolResponse> DeleteItem(GrpcDeleteRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new DeleteUnitItemCommand()
            {
                Id = request.Id,
                UpdateBy = request.UpdateBy
            });
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcBoolResponse() { Success = item };
        }

        #region Method
        private static GrpcUnitItemResponse MapToItemResponse(UnitItemResponse c)
        {
            return new GrpcUnitItemResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description ?? "",
                ValidDuration = c.ValidDuration,
                LiveDuration = c.LiveDuration,
                BenefitType = c.BenefitType,
                BenefitSize = c.BenefitSize ?? "",
                QuantityUpload = c.QuantityUpload,
                Priority = c.Priority,
                ShowInPackage = c.ShowInPackage,
                ShowInPageInDealPrice = c.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = c.ShowInPageInSpirationPrice,
                Active = c.Active,
                Price = (double)c.Price,
                DiscountPrice = (double)c.DiscountPrice,
                DiscountPercent = c.DiscountPercent
            };
        }
        #endregion
    }
}
