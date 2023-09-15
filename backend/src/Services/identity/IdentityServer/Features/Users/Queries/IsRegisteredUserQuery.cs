namespace IdentityServer.Features.Users.Queries
{
    public class IsRegisteredUserQuery : IQuery<bool>
    {
        public required string EmailOrPhone { get; set; }
    }
    public sealed class IsRegisteredUserQueryHandler : IQueryHandler<IsRegisteredUserQuery, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        public IsRegisteredUserQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }
        public async ValueTask<bool> Handle(IsRegisteredUserQuery query, CancellationToken cancellationToken)
        {
            bool res = false;
            if (ValidateInput.IsValidEmail(query.EmailOrPhone))
            {
                res = await _userManager.Users
                    .AnyAsync(c => c.Email == query.EmailOrPhone, cancellationToken);
            }
            else if (ValidateInput.IsValidPhoneNumber(query.EmailOrPhone))
            {
                res = await _userManager.Users
                    .AnyAsync(c => c.PhoneNumber == query.EmailOrPhone, cancellationToken);
            }

            return res;
        }
    }
}
