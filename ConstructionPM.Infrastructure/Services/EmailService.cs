using ConstructionPM.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using ConstructionPM.Application.Interfaces.Services;
using System.Net.Mail;
using System.Net;

namespace ConstructionPM.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendApprovalEmailAsync(
            string toEmail,
            string fullName,
            string tempPassword)
        {
            var subject = "Account Approved - ConstructionPM";

            var body = $@"Hello {fullName},
                             Your ConstructionPM account has been approved.
                            
                            Login credentials:
                            Email: {toEmail}
                            Temporary Password: {tempPassword}
                            
                            Please log in and change your password immediately.
                            https://localhost:7188/api/auth/login
                            
                            Regards,
                            ConstructionPM Team
                            ";
            var massage = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail),
                Subject = subject,
                Body = body,
            };
            massage.To.Add(toEmail);

            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),

                EnableSsl = _settings.EnableSsl
            };

            await smtp.SendMailAsync(massage);
        }
        }

    }
