using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Net;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;

namespace InteractiveCV.Services
{

    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly ILogger _logger;

        public EmailService(IEmailConfiguration emailConfiguration, ILoggerFactory logger)
        {
            _emailConfiguration = emailConfiguration;
            _logger = logger.CreateLogger("Message logger");
        }

        public Task SendEmailAsync(String email, String subject, String message)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_emailConfiguration.fromName, _emailConfiguration.fromAddress));
            msg.To.Add(new MailboxAddress(email));
            msg.Subject = subject;

            msg.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                /* I need to receive the credentials from Google API. Not going to work right now*/

                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                client.Send(msg);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
