using IdentityServer.Dtos.Responses.Users;

namespace IdentityServer.Features.Users.Queries
{
    public class GetUserByEmailOrPhoneQuery : IQuery<BaseWithDataResponse?>
    {
        public required string EmailOrPhone { get; set; }
    }

    public sealed class GetUserByEmailOrPhoneQueryHandler : IQueryHandler<GetUserByEmailOrPhoneQuery, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetUserByEmailOrPhoneQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<BaseWithDataResponse?> Handle(GetUserByEmailOrPhoneQuery query, CancellationToken cancellationToken)
        {
            UserItemResponse? user = null;
            if (ValidateInput.IsValidEmail(query.EmailOrPhone))
            {
                user = await _userManager.Users
                    .Select(c => new UserItemResponse
                    {
                        UserName = c.UserName,
                        Email = c.Email,
                        EmailConfirmed = c.EmailConfirmed,
                        PhoneNumber = c.PhoneNumber,
                        PhoneNumberConfirmed = c.PhoneNumberConfirmed,
                        Active = c.Active,
                    }).AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Email == query.EmailOrPhone, cancellationToken);
            }
            else if (ValidateInput.IsValidPhoneNumber(query.EmailOrPhone))
            {
                user = await _userManager.Users
                     .Select(c => new UserItemResponse
                     {
                         UserName = c.UserName,
                         Email = c.Email,
                         EmailConfirmed = c.EmailConfirmed,
                         PhoneNumber = c.PhoneNumber,
                         PhoneNumberConfirmed = c.PhoneNumberConfirmed,
                         Active = c.Active,
                     }).AsNoTracking()
                .FirstOrDefaultAsync(c => c.PhoneNumber == query.EmailOrPhone, cancellationToken);
            }
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
