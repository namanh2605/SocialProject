using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SocialProject.Data.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailPassword;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;

      
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var _smtpServer = _configuration["SmtpSettings:Server"];
            var _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
            var _emailFrom = _configuration["SmtpSettings:Username"];
            var _emailPassword = _configuration["SmtpSettings:Password"];
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email", "Email address cannot be null or empty");
            }

            if (string.IsNullOrEmpty(_emailFrom))
            {
                throw new ArgumentNullException("From email address is not configured.");
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailFrom),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailFrom, _emailPassword);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}