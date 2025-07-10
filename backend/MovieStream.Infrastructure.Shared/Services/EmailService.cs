using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MovieStream.Core.Application.DTOs.Email;
using MovieStream.Core.Application.Interfaces.Services;
using MovieStream.Core.Domain.Settings;
using System.Text;
namespace MovieStream.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings MailSettings { get; }
        private readonly ILogger<EmailService> _logger;
        private readonly IWebHostEnvironment _env;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger,
                            IWebHostEnvironment env)
        {
            MailSettings = mailSettings.Value;
            _logger = logger;
            _env = env;
        }

        public async Task SendAsync(EmailRequest request)
        {   try
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(MailSettings.DisplayName, request.From ?? MailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                var builder = new BodyBuilder();
                if (!string.IsNullOrEmpty(request.HtmlBodyTemplateName))
                {
                    string htmlBody = LoadHtmlTemplate(request.HtmlBodyTemplateName);

                    if (request.TemplateData != null)
                    {
                        var sb = new StringBuilder(htmlBody);
                        foreach (var kvp in request.TemplateData)
                        {
                            sb.Replace($"{{{{{kvp.Key}}}}}", kvp.Value?.ToString() ?? string.Empty);
                        }
                        sb.Replace($"{{CurrentYear}}", DateTime.UtcNow.Year.ToString());
                        builder.HtmlBody = sb.ToString();
                    }
                }
                else 
                {
                    builder.HtmlBody = request.Body;
                }
                email.Body = builder.ToMessageBody();
                await SendEmailAsync(email);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "SMTP Command Error while sending email to {To}: {Message}", request.To, ex.Message);
                throw;
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, "SMTP Protocol Error while sending email to {To}: {Message}", request.To, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email to {To}: {Message}", request.To, ex.Message);
                throw;
            }
        }

        private async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(MailSettings.SmtpHost, MailSettings.SmtpPort, SecureSocketOptions.StartTls).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(MailSettings.SmtpUser) && !string.IsNullOrEmpty(MailSettings.SmtpPass))
                {
                    await smtp.AuthenticateAsync(MailSettings.SmtpUser, MailSettings.SmtpPass).ConfigureAwait(false);
                }
                await smtp.SendAsync(email).ConfigureAwait(false);
            }
            finally
            {
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
        }

        private string LoadHtmlTemplate(string templateName)
        {
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogError($"Email template '{templateName}.html' not found at path '{templatePath}'");
                throw new FileNotFoundException($"Email template {templateName}.html not found. Searched at: {templatePath}", templatePath);
            }
            return File.ReadAllText(templatePath);
        }
    }
}
