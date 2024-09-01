using MailKit.Net.Smtp;
using MimeKit;
using ServiceRequestApi.Models;
using System.Threading.Tasks;

namespace ServiceRequestApi.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly string _smtpServer = "smtp.example.com"; // Configure your SMTP server
        private readonly int _smtpPort = 587; // Configure your SMTP port
        private readonly string _smtpUser = "user@example.com"; // Configure your SMTP user
        private readonly string _smtpPass = "password"; // Configure your SMTP password

        public async Task SendNotificationAsync(ServiceRequest serviceRequest)
        {
            if (serviceRequest.CurrentStatus == CurrentStatus.Complete)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Service Request System", _smtpUser));
                message.To.Add(new MailboxAddress(serviceRequest.CreatedBy, "user@example.com")); // Replace with actual user email
                message.Subject = "Service Request Closed";
                message.Body = new TextPart("plain")
                {
                    Text = $"Your service request with ID {serviceRequest.Id} has been closed."
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
