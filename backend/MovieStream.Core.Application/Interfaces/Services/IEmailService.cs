using MovieStream.Core.Application.DTOs.Email;
using MovieStream.Core.Domain.Settings;

namespace MovieStream.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}