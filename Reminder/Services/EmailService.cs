using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Reminder.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Send(string receiver, string sender, string subject, string body)
        {
            var host = configuration.GetSection("MailSettings:Host").Value;
            var username = configuration.GetSection("MailSettings:Username").Value;
            var password = configuration.GetSection("MailSettings:Password").Value;
            var port = int.Parse(configuration.GetSection("MailSettings:Port").Value);

            using (var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            })
            {
                try
                {
                    // Specify the email sender.
                    // Create a mailing address that includes a UTF8 character
                    // in the display name.
                    MailAddress from = new MailAddress(sender);
                    // Set destinations for the email message.
                    MailAddress to = new MailAddress(receiver);
                    // Specify the message content.
                    MailMessage message = new MailMessage(from, to);
                    message.Body = body;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = subject;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    // The userState can be any object that allows your callback
                    // method to identify this send operation.
                    // For this example, the userToken is a string constant.
                    string userState = "test message1";
                    client.SendAsync(message, userState);
                    Console.WriteLine($"Sending message... from {sender} to {receiver}.");

                    message.Dispose();
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }
    }
}
