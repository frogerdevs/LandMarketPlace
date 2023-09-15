using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcSubscription;

namespace Subscription.Api.Grpc
{
    public class VoucherService : VoucherGrpc.VoucherGrpcBase
    {
        readonly IMediator _mediator;
        public VoucherService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcVoucherResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetVouchersQuery());
            var res = new GrpcVoucherResponse();

            res.Data.AddRange(items?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        public override async Task<GrpcVoucherItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetVoucherByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcVoucherItemResponse();
            }

            var res = MapToItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcVoucherItemResponse> AddItem(GrpcAddVoucherRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new AddVoucherCommand()
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                StarDate = request.StarDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                Duration = request.Duration,
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
            return new GrpcVoucherItemResponse();
        }
        public override async Task<GrpcVoucherItemResponse> EditItem(GrpcEditVoucherRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new EditVoucherCommand()
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                StarDate = request.StarDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                Duration = request.Duration,
                Active = request.Active,
                UpdateBy = request.UpdateBy,
            });
            if (item != null)
            {
                var res = MapToItemResponse(item);
                context.Status = new Status(StatusCode.OK, $"Success");
                return res;
            }
            context.Status = new Status(StatusCode.NotFound, $" Item do not exist");
            return new GrpcVoucherItemResponse();
        }
        public override async Task<GrpcBoolResponse> DeleteItem(GrpcDeleteRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new DeleteVoucherCommand()
            {
                Id = request.Id,
                UpdateBy = request.UpdateBy
            });
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcBoolResponse() { Success = item };
        }

        #region Method
        private static GrpcVoucherItemResponse MapToItemResponse(VoucherItem c)
        {
            return new GrpcVoucherItemResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name ?? "",
                Description = c.Description ?? "",
                StarDate = Timestamp.FromDateTimeOffset(c.StarDate ?? default),
                EndDate = Timestamp.FromDateTimeOffset(c.EndDate ?? default),
                Duration = c.Duration,
                Active = c.Active
            };
        }
        #endregion
    }
}
