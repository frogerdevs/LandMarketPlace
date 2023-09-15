namespace IdentityServer.Features.Users.Queries
{
    public class GetUsersQuery : IQuery<object>
    {
    }

    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, object>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<object> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Select(c => new
            {
                c.Id,
                UserName = c.UserName,
                Email = c.Email,
                EmailConfirmed = c.EmailConfirmed,
                PhoneNumber = c.PhoneNumber,
                PhoneNumberConfirmed = c.PhoneNumberConfirmed,
                Active = c.Active,
                IsCompany = (c.SellerProfile != null) && c.SellerProfile.IsCompany,
                BrandName = (c.SellerProfile != null) ? c.SellerProfile.BrandName : null,
                CategoryId = c.SellerCategoryId,
            }).ToListAsync(cancellationToken);

            var res = new
            {
                Success = true,
                Message = "Success Get Data",
                Count = users.Count,
                Data = users
            };
            return res;
        }

    }

}
