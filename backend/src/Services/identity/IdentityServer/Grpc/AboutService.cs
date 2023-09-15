using Grpc.Core;
using GrpcIdentity;

namespace IdentityServer.Grpc
{
    public class AboutService : AboutGrpc.AboutGrpcBase
    {
        private readonly IMediator _mediator;
        public AboutService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcAboutResponse> GetAboutByUser(GrpcByIdRequest request, ServerCallContext context)
        {
            var user = await _mediator.Send(new GetAboutByUserQuery() { UserId = request.Id });
            if (user == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcAboutResponse();
            }

            context.Status = new Status(StatusCode.OK, $"Success");

            return MapToAbout(user);
        }

        private static GrpcAboutResponse MapToAbout(AboutResponse user)
        {
            return new GrpcAboutResponse
            {
                UserId = user.UserId,
                Email = user.Email ?? "",
                PriceFrom = user.PriceFrom ?? "",
                PriceTo = user.PriceTo ?? "",
                Description = user.Description ?? "",
                Vision = user.Vision ?? "",
                Mission = user.Mission ?? "",
                Contact = user.Contact ?? "",
                WhatsApp = user.WhatsApp ?? "",
                Facebook = user.Facebook ?? "",
                Instagram = user.Instagram ?? "",
                Twitter = user.Twitter ?? "",
                Tiktok = user.Tiktok ?? "",
                Website = user.Website ?? "",
            };
        }

        public override async Task<GrpcAboutResponse> GetAboutByEmail(GrpcByIdRequest request, ServerCallContext context)
        {
            var user = await _mediator.Send(new GetAboutByEmailQuery() { Email = request.Id });
            if (user == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcAboutResponse();
            }

            context.Status = new Status(StatusCode.OK, $"Success");

            return MapToAbout(user);
        }
    }
}
