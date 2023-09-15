namespace IdentityServer.Features.About.Queries
{
    public class GetAboutByEmailQuery : IQuery<AboutResponse?>
    {
        public required string Email { get; set; }
    }

    public sealed class GetAboutByEmailQueryHandler : IQueryHandler<GetAboutByEmailQuery, AboutResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetAboutByEmailQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<AboutResponse?> Handle(GetAboutByEmailQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(x => x.SellerProfile).Include(c => c.SellerEvents).Include(c => c.SellerAchievements)
            .Select(c => new AboutResponse
            {
                UserId = c.Id,
                Email = c.Email,
                Vision = c.SellerProfile == null ? null : c.SellerProfile.Vision,
                Mission = c.SellerProfile == null ? null : c.SellerProfile.Mission,
                Contact = c.SellerProfile == null ? null : c.SellerProfile.Contact,
                WhatsApp = c.SellerProfile == null ? null : c.SellerProfile.WhatsApp,
                Facebook = c.SellerProfile == null ? null : c.SellerProfile.Facebook,
                Instagram = c.SellerProfile == null ? null : c.SellerProfile.Instagram,
                Twitter = c.SellerProfile == null ? null : c.SellerProfile.Twitter,
                Tiktok = c.SellerProfile == null ? null : c.SellerProfile.Tiktok,
                Website = c.SellerProfile == null ? null : c.SellerProfile.Website,
                SellerEvents = EventMaps(c.SellerEvents),
                SellerAchievements = AchievementMaps(c.SellerAchievements)
            }).AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == query.Email, cancellationToken);

            return user;
        }

        private static ICollection<SellerAchievementResponse>? AchievementMaps(ICollection<SellerAchievement>? sellerAchievements)
        {
            return sellerAchievements?.Select(sellerAchievement => new SellerAchievementResponse
            {
                Title = sellerAchievement.Title,
                Image = sellerAchievement.Image,
            }).ToList();
        }

        private static ICollection<SellerEventResponse>? EventMaps(ICollection<SellerEvent>? sellerEvents)
        {
            return sellerEvents?.Select(sellerEvent => new SellerEventResponse
            {
                Title = sellerEvent.Title,
                Images = EventImageMap(sellerEvent.Images)
            }).ToList();
        }

        private static ICollection<SellerEventImageResponse>? EventImageMap(ICollection<SellerEventImage>? images)
        {
            return images?.Select(image => new SellerEventImageResponse
            {
                Image = image.Image
            }).ToList();
        }
    }
}
