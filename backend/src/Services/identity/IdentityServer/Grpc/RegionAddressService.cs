using Grpc.Core;
using GrpcIdentity;

namespace IdentityServer.Grpc
{
    public class RegionAddressService : RegionAddressGrpc.RegionAddressGrpcBase
    {
        private readonly IMediator _mediator;
        public RegionAddressService(IMediator mediator)
        {

            _mediator = mediator;

        }
        public override async Task<GrpcAddressItemResponse> GetProvinceById(GrpcByIdRequest request, ServerCallContext context)
        {
            var city = await _mediator.Send(new GetProvinceByIdQuery() { Id = request.Id });
            if (city == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data dengan Id :{request.Id} tidak ditemukan");
                return new GrpcAddressItemResponse();
            }
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcAddressItemResponse { Id = city.Id, Name = city.Name };
        }
        public override async Task<GrpcAddressItemResponse> GetCityById(GrpcByIdRequest request, ServerCallContext context)
        {
            var city = await _mediator.Send(new GetCityByIdQuery() { Id = request.Id });
            if (city == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data dengan Id :{request.Id} tidak ditemukan");
                return new GrpcAddressItemResponse();
            }
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcAddressItemResponse { Id = city.Id, Name = city.Name };
        }
        public override async Task<GrpcAddressItemResponse> GetDisctrictById(GrpcByIdRequest request, ServerCallContext context)
        {
            var city = await _mediator.Send(new GetDistrictByIdQuery() { Id = request.Id });
            if (city == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data dengan Id :{request.Id} tidak ditemukan");
                return new GrpcAddressItemResponse();
            }
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcAddressItemResponse { Id = city.Id, Name = city.Name };
        }
        public override async Task<GrpcAddressItemResponse> GetSubDisctrictById(GrpcByIdRequest request, ServerCallContext context)
        {
            var city = await _mediator.Send(new GetSubDistrictByIdQuery() { Id = request.Id });
            if (city == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data dengan Id :{request.Id} tidak ditemukan");
                return new GrpcAddressItemResponse();
            }
            context.Status = new Status(StatusCode.OK, $"Success");
            return new GrpcAddressItemResponse { Id = city.Id, Name = city.Name };
        }
    }
}
