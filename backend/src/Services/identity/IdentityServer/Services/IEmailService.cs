using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellation = default);
    }
}
