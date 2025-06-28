using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MovieStream.Core.Application.DTOs.Email;
using MovieStream.Core.Application.Interfaces.Services;
using MovieStream.Core.Domain.Settings;
namespace MovieStream.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings MailSettings { get; }
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            MailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(MailSettings.DisplayName, request.From ?? MailSettings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            var builder = new BodyBuilder { HtmlBody = request.Body };
            email.Body = builder.ToMessageBody();

            try
            {
                await SendEmailAsync(email);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "SMTP Command Error while sending email: {Message}", ex.Message);
                throw;
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, "SMTP Protocol Error while sending email: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email: {Message}", ex.Message);
                throw;
            }
        }

        private async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(MailSettings.SmtpHost, MailSettings.SmtpPort, SecureSocketOptions.StartTls).ConfigureAwait(false);
                await smtp.AuthenticateAsync(MailSettings.SmtpUser, MailSettings.SmtpPass).ConfigureAwait(false);
                await smtp.SendAsync(email).ConfigureAwait(false);
            }
            finally
            {
                await smtp.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
