using Grpc.Core;
using GrpcSubscription;

namespace Subscription.Api.Grpc
{
    public class UnitTypeService : UnitTypeGrpc.UnitTypeGrpcBase
    {
        readonly IMediator _mediator;
        public UnitTypeService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcUnitTypeResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {

            var items = await _mediator.Send(new GetUnitTypesQuery());
            var res = new GrpcUnitTypeResponse();

            res.Data.AddRange(items?.Select(c => MapToItemResponse(c)).ToList());
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }


        public override async Task<GrpcUnitTypeItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetUnitTypeByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcUnitTypeItemResponse();
            }

            var res = MapToItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        public override async Task<GrpcUnitTypeItemResponse> AddItem(GrpcAddUnitTypeRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new AddUnitTypeCommand()
            {
                Name = request.Name,
                Description = request.Description,
                Size = request.Size,
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
            return new GrpcUnitTypeItemResponse();
        }
        public override async Task<GrpcUnitTypeItemResponse> EditItem(GrpcEditUnitTypeRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new EditUnitTypeCommand()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Size = request.Size,
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
            return new GrpcUnitTypeItemResponse();
        }
        public override async Task<GrpcBoolResponse> DeleteItem(GrpcDeleteRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new DeleteUnitTypeCommand()
            {
                Id = request.Id,
                UpdateBy = request.UpdateBy
            });
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcBoolResponse() { Success = item };
        }

        #region Method

        private static GrpcUnitTypeItemResponse MapToItemResponse(UnitTypeItem item)
        {
            return new GrpcUnitTypeItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description ?? "",
                Size = item.Size ?? "",
                Active = item.Active
            };
        }
        private static GrpcUnitTypeItemResponse MapToListResponse(UnitTypeItem c)
        {
            return new GrpcUnitTypeItemResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? "",
                Size = c.Size ?? "",
                Active = c.Active
            };
        }
        #endregion
    }
}
