using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWD.Data.Entities;
using System.Net.Mail;
using System.Net;


namespace SWD.Service.Services
{
    public class SmtpEmailSender : IEmailSender<User>
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IConfiguration config, ILogger<SmtpEmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            string subject = "Confirm Your Email";
            string body = $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            string subject = "Reset Your Password";
            string body = $"<p>Reset your password by clicking <a href='{resetLink}'>here</a>.</p>";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            string subject = "Your Password Reset Code";
            string body = $"<p>Your password reset code is: <strong>{resetCode}</strong></p>";
            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _config["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var smtpUser = _config["EmailSettings:SmtpUser"];
            var smtpPass = _config["EmailSettings:SmtpPass"];
            var fromEmail = _config["EmailSettings:FromEmail"];
            var fromName = _config["EmailSettings:FromName"];

            try
            {
                var client = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }
}
