using Hangfire;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailData(Email email, CancellationToken token = default)
        {
            using var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(
                    _emailSettings.UserName,
                    _emailSettings.Password),

                EnableSsl = _emailSettings.UseSSL,

                Timeout = 30000 // 30 seconds
            };

            using var message = new MailMessage
            {
                From = new MailAddress(
                    _emailSettings.FromAddress,
                    _emailSettings.FromName),

                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            message.To.Add(email.To);

            await smtp.SendMailAsync(message, token);

            return true;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task<bool> SendEmail(Email email)
        {
            return await SendEmailData(email);
        }
    }
}
