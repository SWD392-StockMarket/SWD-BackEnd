using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWD.Data.Entities;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;


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
            string body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
        <div style='max-width: 600px; background: white; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1);'>
            <h2 style='color: #333;'>Confirm Your Email</h2>
            <p style='font-size: 16px; color: #555;'>Hello {user.UserName},</p>
            <p style='font-size: 16px; color: #555;'>Please confirm your email by clicking the button below:</p>
            <a href='{confirmationLink}' style='display: inline-block; padding: 10px 20px; color: #fff; background: #28a745; text-decoration: none; border-radius: 5px; font-size: 16px;'>Confirm Email</a>
            <p style='font-size: 14px; color: #888;'>If you did not request this, please ignore this email.</p>
        </div>
    </div>";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            string subject = "Reset Your Password";
            string body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
        <div style='max-width: 600px; background: white; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1);'>
            <h2 style='color: #333;'>Reset Your Password</h2>
            <p style='font-size: 16px; color: #555;'>Hello {user.UserName},</p>
            <p style='font-size: 16px; color: #555;'>You can reset your password by clicking the button below:</p>
            <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; color: #fff; background: #dc3545; text-decoration: none; border-radius: 5px; font-size: 16px;'>Reset Password</a>
            <p style='font-size: 14px; color: #888;'>If you did not request this, please ignore this email.</p>
        </div>
    </div>";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            string subject = "Your Password Reset Code";
            string body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
        <div style='max-width: 600px; background: white; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1);'>
            <h2 style='color: #333;'>Password Reset Code</h2>
            <p style='font-size: 16px; color: #555;'>Hello {user.UserName},</p>
            <p style='font-size: 16px; color: #555;'>Your password reset code is:</p>
            <div style='font-size: 20px; font-weight: bold; color: #007bff; background: #e9ecef; padding: 10px; display: inline-block; border-radius: 5px;'>{resetCode}</div>
            <p style='font-size: 14px; color: #888;'>Use this code to reset your password. It will expire soon.</p>
        </div>
    </div>";
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
