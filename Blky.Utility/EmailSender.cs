using Microsoft.AspNetCore.Identity.UI.Services;

namespace Blky.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //face email sender
            return Task.CompletedTask;
        }
    }
}
