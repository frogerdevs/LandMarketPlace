using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Profile.Queries
{

    public class GetProfileLandersByEmailQuery : IQuery<BaseWithDataResponse?>
    {
        public required string Email { get; set; }
    }

    public sealed class GetProfileLandersByEmailQueryHandler : IQueryHandler<GetProfileLandersByEmailQuery, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetProfileLandersByEmailQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<BaseWithDataResponse?> Handle(GetProfileLandersByEmailQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                  .Select(c => new
                  {
                      IsSeller = c.IsSeller,
                      UserName = c.UserName,
                      Email = c.Email,
                      EmailConfirmed = c.EmailConfirmed,
                      PhoneNumber = c.PhoneNumber,
                      PhoneNumberConfirmed = c.PhoneNumberConfirmed,
                      Active = c.Active,
                      NewsLetter = c.NewsLetter,
                      c.FirstName,
                      c.LastName,
                      c.ImageUrl
                  }).AsNoTracking()
                  .FirstOrDefaultAsync(c => c.Email == query.Email, cancellationToken);

            if (user == null)
            {
                return null;
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = user
            };
            return res;
        }
    }

}
