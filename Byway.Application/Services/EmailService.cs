using Byway.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendWelcomeEmailAsync(string email, string firstName)
        {
            var subject = "Welcome to Byway! 🎉";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9; border-radius: 10px;'>
        <h2 style='color: #667eea;'>Dear {firstName}! 🎉</h2>
        
        <h3 style='color: #667eea;'>Welcome aboard! 🎓</h3>
        
        <p style='font-size: 16px;'>
            Your learning journey starts here. Let's grow your skills together!
        </p>
        
        <div style='margin: 30px 0; padding: 20px; background-color: white; border-radius: 5px;'>
            <p style='margin: 0;'><strong>What's next?</strong></p>
            <ul style='margin: 10px 0;'>
                <li>Browse our courses</li>
                <li>Start learning today</li>
                <li>Join our community</li>
            </ul>
        </div>
        
        <p style='margin-top: 30px; color: #666;'>
            Best regards,<br>
            <strong>The Byway Team</strong>
        </p>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, body);
            
            Console.WriteLine($"✅ Welcome Email sent successfully to: {email}");
        }

        public async Task SendPaymentConfirmationEmailAsync(string email, string firstName, decimal totalAmount)
        {
            var subject = "Payment Confirmation - Thank You! 🎉";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9; border-radius: 10px;'>
        <h2 style='color: #667eea;'>Dear {firstName}! 🎉</h2>
        
        <h3 style='color: #4caf50;'>Thank you for your purchase!</h3>
        
        <p style='font-size: 16px;'>
            Your courses are now available in your dashboard. Best of luck on your learning journey!
        </p>
        
        <div style='margin: 30px 0; padding: 20px; background-color: white; border-radius: 5px;'>
            <h4 style='margin: 0 0 10px 0; color: #667eea;'>Order Summary</h4>
            <p style='margin: 5px 0;'>
                <strong>Total Amount:</strong> 
                <span style='color: #4caf50; font-size: 20px;'>${totalAmount:F2}</span>
            </p>
            <p style='margin: 5px 0; color: #666; font-size: 14px;'>
                Thank you for choosing Byway!
            </p>
        </div>
        
        <div style='margin: 20px 0; padding: 15px; background-color: #e3f2fd; border-left: 4px solid #2196f3; border-radius: 5px;'>
            <p style='margin: 0;'><strong>💡 Quick Tip:</strong></p>
            <p style='margin: 5px 0; font-size: 14px;'>
                Access your courses anytime from your dashboard. Start learning at your own pace!
            </p>
        </div>
        
        <p style='margin-top: 30px; color: #666;'>
            Best regards,<br>
            <strong>The Byway Team</strong>
        </p>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, body);
            
            Console.WriteLine($"✅ Payment Confirmation Email sent successfully to: {email}");
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                // Get email settings from configuration
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? username;
                var fromName = _configuration["EmailSettings:FromName"] ?? "Byway Learning Platform";

                // Check if email settings are configured
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("⚠️ Email settings not configured. Email will not be sent.");
                    Console.WriteLine($"📧 Email would be sent to: {toEmail}");
                    Console.WriteLine($"📧 Subject: {subject}");
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine($"✅ Email sent successfully to: {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send email to {toEmail}");
                Console.WriteLine($"❌ Error: {ex.Message}");
                // Don't throw - we don't want email failures to break the application
            }
        }
    }
}