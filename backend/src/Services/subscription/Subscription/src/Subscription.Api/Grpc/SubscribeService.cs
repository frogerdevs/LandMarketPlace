using Grpc.Core;
using GrpcSubscription;

namespace Subscription.Api.Grpc
{
    public class SubscribeService : SubscribeGrpc.SubscribeGrpcBase
    {
        readonly IMediator _mediator;
        public SubscribeService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcSubscribeResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetSubscribesQuery());
            var res = new GrpcSubscribeResponse();
            res.Data.AddRange(items?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");
            return res;
        }

        public override async Task<GrpcSubscribeItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetSubscribeByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcSubscribeItemResponse();
            }

            var res = MapToItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcSubscribeItemResponse> AddItem(GrpcAddSubscribeRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new AddSubscribeCommand()
            {
                Name = request.Name,
                Description = request.Description,
                Price = (decimal)request.Price,
                DurationDays = request.DurationDays,
                UpgradableFrom = request.UpgradableFrom,
                DiscountPrice = (decimal)request.DiscountPrice,
                DiscountPercent = request.DiscountPercent,
                Active = request.Active,
                CreatedBy = request.CreatedBy
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcSubscribeItemResponse();
        }
        public override async Task<GrpcSubscribeItemResponse> EditItem(GrpcEditSubscribeRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new EditSubscribeCommand()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = (decimal)request.Price,
                DurationDays = request.DurationDays,
                UpgradableFrom = request.UpgradableFrom,
                DiscountPrice = (decimal)request.DiscountPrice,
                DiscountPercent = request.DiscountPercent,
                Active = request.Active,
                UpdateBy = request.UpdateBy
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcSubscribeItemResponse();
        }
        public override async Task<GrpcBoolResponse> DeleteItem(GrpcDeleteRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new DeleteSubscribeCommand()
            {
                Id = request.Id,
                UpdateBy = request.UpdateBy
            });
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcBoolResponse() { Success = item };
        }

        #region Method
        private GrpcSubscribeItemResponse MapToItemResponse(SubscribeItem c)
        {
            return new GrpcSubscribeItemResponse
            {
                Id = c.Id,
                Name = c.Name ?? throw new NullReferenceException("The value of 'c.Name' should not be null"),
                Description = c.Description ?? throw new NullReferenceException("The value of 'c.Description' should not be null"),
                Price = (double)c.Price,
                DurationDays = c.DurationDays,
                UpgradableFrom = c.UpgradableFrom ?? throw new NullReferenceException("The value of 'c.UpgradableFrom' should not be null"),
                DiscountPrice = (double)c.DiscountPrice,
                DiscountPercent = c.DiscountPercent,
                Active = c.Active
            };
        }

        #endregion
    }
}
