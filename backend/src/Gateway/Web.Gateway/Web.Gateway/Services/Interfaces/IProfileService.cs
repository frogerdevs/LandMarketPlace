using Web.Gateway.Dto.Request.Profiles;
using Web.Gateway.Dto.Response.Profile;

namespace Web.Gateway.Services.Interfaces
{
    public interface IProfileService
    {
        ValueTask<HttpResponseMessage> GetLandersAsync(string email, CancellationToken cancellationToken);
        ValueTask<ProfileMerchantResponse?> GetMerchantAsync(string email, CancellationToken cancellationToken);
        ValueTask<ProfileByBrandSlugResponse?> GetProfileBySlugAsync(string slug, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PutLandersAsync(EditProfileLandersRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PutMerchantAsync(EditProfileMerchantRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> AddMerchantVerificationAsync(AddMerchantVerificationRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken);

    }
}
