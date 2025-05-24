using AppClinica.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AppClinica.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null)
        {
            var message = new MailMessage(_settings.UserName, toEmail, subject, body);
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                message.Attachments.Add(new Attachment(attachmentPath));
            }

            using var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            await smtp.SendMailAsync(message);
        }
    }
}
