using Grpc.Core;
using GrpcIdentity;
using IdentityServer.Features.Profile.Queries;
using IdentityServer.Features.Users.Queries;

namespace IdentityServer.Grpc
{
    public class UserService : UserGrpc.UserGrpcBase
    {
        private readonly IMediator _mediator;
        public UserService(IMediator mediator)
        {

            _mediator = mediator;

        }
        public override async Task<GrpcUserBrandResponse> GetUserBrand(GrpcByIdRequest request, ServerCallContext context)
        {
            var user = await _mediator.Send(new GetUserByIdQuery() { Id = request.Id });
            if (user == null || user.Data == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcUserBrandResponse();
            }
            var res = new GrpcUserBrandResponse
            {
                BrandName = user.Data.BrandName,
                BrandSlug = user.Data.BrandSlug
            };
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcContactPersonResponse> GetContactPerson(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetContactPersonMerchantQuery() { UserId = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcContactPersonResponse();
            }
            var res = MapToContactPersonResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        public override async Task<GrpcProfileMerchantResponse> GetProfileBrandBySlug(GrpcBySlugRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProfileBrandBySlugQuery() { Slug = request.Slug });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcProfileMerchantResponse();
            }
            var res = MapToProfileMerchantResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        private static GrpcProfileMerchantResponse MapToProfileMerchantResponse(ProfileMerchantResponse item)
        {
            return new GrpcProfileMerchantResponse
            {
                UserId = item.UserId,
                ImageUrl = item.ImageUrl ?? "",
                BrandName = item.BrandName ?? "",
                BrandSlug = item.BrandSlug ?? "",
                CategoryId = item.CategoryId ?? "",
                Address = item.Address ?? "",
                Province = item.Province ?? "",
                City = item.City ?? "",
                District = item.District ?? "",
                SubDistrict = item.SubDistrict ?? "",
                PostCode = item.PostCode ?? "",
                Verified = item.Verified
            };
        }

        private static GrpcContactPersonResponse MapToContactPersonResponse(ContactPersonResponse item)
        {
            return new GrpcContactPersonResponse
            {
                UserId = item.UserId ?? "",
                BrandName = item.BrandName ?? "",
                BrandSlug = item.BrandSlug ?? "",
                ImageUrl = item.ImageUrl ?? "",
                Email = item.Email ?? "",
                Contact = item.Contact ?? "",
                WhatsApp = item.WhatsApp ?? "",
                Facebook = item.Facebook ?? "",
                Instagram = item.Instagram ?? "",
                Twitter = item.Twitter ?? "",
                Tiktok = item.Tiktok ?? "",
                Website = item.Website ?? "",
            };
        }
    }
}
