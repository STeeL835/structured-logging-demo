using Microsoft.Extensions.Logging;

namespace StructuredLoggingDemo.WebApi.Emailing
{
    public interface IEmailService
    {
        void SendEmail(string email, string text);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }


        public void SendEmail(string email, string text)
        {
            // _logger.LogDebug("Sending email to {EmailAddress}", email);

            var client = new EmailClient();

            client.SendEmail(email, text);
        }
    }
}
