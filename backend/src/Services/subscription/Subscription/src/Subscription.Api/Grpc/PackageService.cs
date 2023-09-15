using Grpc.Core;
using GrpcSubscription;

namespace Subscription.Api.Grpc
{
    public class PackageService : PackageGrpc.PackageGrpcBase
    {
        readonly IMediator _mediator;
        public PackageService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcPackageResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetPackagesQuery());
            var res = new GrpcPackageResponse();
            res.Data.AddRange(items?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");
            return res;
        }

        public override async Task<GrpcPackageItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetPackageByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcPackageItemResponse();
            }

            var res = MapToItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcPackageItemResponse> AddItem(GrpcAddPackageRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new AddPackageCommand()
            {
                Title = request.Title,
                Description = request.Description,
                Duration = request.Duration,
                Price = (decimal)request.Price,
                DiscountPrice = (decimal)request.DiscountPrice,
                DiscountPercent = request.DiscountPercent,
                Active = request.Active,
                CreatedBy = request.CreatedBy,
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcPackageItemResponse();
        }
        public override async Task<GrpcPackageItemResponse> EditItem(GrpcEditPackageRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new EditPackageCommand()
            {
                Id = request.Id,
                Title = request.Name,
                Description = request.Description,
                Duration = request.Duration,
                Price = (decimal)request.Price,
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
            return new GrpcPackageItemResponse();
        }
        public override async Task<GrpcBoolResponse> DeleteItem(GrpcDeleteRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new DeletePackageCommand()
            {
                Id = request.Id,
                UpdateBy = request.UpdateBy
            });
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcBoolResponse() { Success = item };
        }
        #region Method
        private static GrpcPackageItemResponse MapToItemResponse(PackageItem c)
        {
            return new GrpcPackageItemResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description ?? "",
                Duration = c.Duration,
                Price = (double)c.Price,
                DiscountPrice = (double)c.DiscountPrice,
                DiscountPercent = c.DiscountPercent,
                Active = c.Active
            };
        }

        #endregion
    }
}
