using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace BookingSystem.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Simulated email to {email} | Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}
