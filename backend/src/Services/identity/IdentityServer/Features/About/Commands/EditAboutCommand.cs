using IdentityServer.Dtos.Requests.Abouts;

namespace IdentityServer.Features.About.Commands
{
    public class EditAboutCommand : AboutRequest, ICommand<BaseWithDataResponse?>
    {
    }
    public sealed class EditAboutCommandHandler : ICommandHandler<EditAboutCommand, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditAboutCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<BaseWithDataResponse?> Handle(EditAboutCommand command, CancellationToken cancellationToken)
        {

            var existuser = await _userManager.Users.Include(sp => sp.SellerProfile).Include(c => c.SellerEvents).Include(c => c.SellerAchievements)
                .FirstOrDefaultAsync(c => c.Email == command.Email, cancellationToken: cancellationToken);
            if (existuser == null)
            {
                return null;
            }
            if (existuser.SellerProfile == null)
            {
                existuser.SellerProfile = NewSellerProfile(command);
            }
            else
            {
                existuser.SellerProfile.Vision = command.Vision;
                existuser.SellerProfile.Mission = command.Mission;
                existuser.SellerProfile.Contact = command.Contact;
                existuser.SellerProfile.WhatsApp = command.WhatsApp;
                existuser.SellerProfile.Facebook = command.Facebook;
                existuser.SellerProfile.Instagram = command.Instagram;
                existuser.SellerProfile.Twitter = command.Twitter;
                existuser.SellerProfile.Tiktok = command.Tiktok;
                existuser.SellerProfile.Website = command.Website;
            }
            existuser.SellerEvents?.Clear();
            existuser.SellerEvents = NewSellerEvent(existuser.Id, command.SellerEvents);

            existuser.SellerAchievements?.Clear();
            existuser.SellerAchievements = NewSellerAchievement(existuser.Id, command.SellerAchievements);

            var identityResult = await _userManager.UpdateAsync(existuser);

            if (identityResult.Errors.Any())
            {
                return new BaseWithDataResponse
                {
                    Success = false,
                    Message = string.Join(", ", identityResult.Errors.Select(e => e.Description)),
                    Data = null
                };
            }

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Berhasil",
                Data = new
                {
                    Email = existuser.Email,
                    EmailConfirmed = existuser.EmailConfirmed,
                }
            };
        }

        private static ICollection<SellerAchievement>? NewSellerAchievement(string userId, ICollection<SellerAchievementRequest>? sellerAchievements)
        {
            return sellerAchievements?.Select(sellerAchievement => new SellerAchievement
            {
                UserId = userId,
                Title = sellerAchievement.Title,
                Image = sellerAchievement.Images
            }).ToList();
        }

        private static ICollection<SellerEvent>? NewSellerEvent(string userId, ICollection<SellerEventRequest>? sellerEvents)
        {
            return sellerEvents?.Select(sellerEvent =>
            {
                var events = new SellerEvent
                {
                    UserId = userId,
                    Title = sellerEvent.Title,
                };
                events.Images = NewSellerEventImage(events.Id, sellerEvent.Images);
                return events;
            }).ToList();
        }

        private static ICollection<SellerEventImage>? NewSellerEventImage(string sellerEventId, ICollection<SellerEventImageRequest>? sellerEventsImage)
        {
            return sellerEventsImage?.Select(image => new SellerEventImage
            {
                SellerEventId = sellerEventId,
                Image = image.Image
            }).ToList();
        }

        private static UserProfile NewSellerProfile(EditAboutCommand command)
        {
            return new UserProfile
            {
                Vision = command.Vision,
                Mission = command.Mission,
                Contact = command.Contact,
                WhatsApp = command.WhatsApp,
                Facebook = command.Facebook,
                Instagram = command.Instagram,
                Twitter = command.Twitter,
                Tiktok = command.Tiktok,
                Website = command.Website
            };
        }
    }

}
