using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ICEDT_TamilApp.Infrastructure.Services
{
    public class LogEmailService : IEmailService
    {
        private readonly ILogger<LogEmailService> _logger;

        public LogEmailService(ILogger<LogEmailService> logger) => _logger = logger;

        public Task SendEmailAsync(string toEmail, string subject, string body)
        {
            _logger.LogWarning("---- SENDING EMAIL (LOG ONLY) ----");
            _logger.LogInformation("To: {ToEmail}", toEmail);
            _logger.LogInformation("Subject: {Subject}", subject);
            _logger.LogInformation("Body:\n{Body}", body);
            _logger.LogWarning("---------------------------------");
            return Task.CompletedTask;
        }
    }
}
