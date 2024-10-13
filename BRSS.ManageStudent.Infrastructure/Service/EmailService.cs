using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BRSS.ManageStudent.Application.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:Name"], "main.brightsoft@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using var client = new SmtpClient();
        try
        {
            // Connect to the SMTP server
            await client.ConnectAsync(
                _configuration["EmailSettings:Host"],
                int.Parse(_configuration["EmailSettings:Port"] ?? throw new InvalidOperationException("Email port not configured")),
                SecureSocketOptions.StartTls);

            // Authenticate with the SMTP server
            await client.AuthenticateAsync(
                _configuration["EmailSettings:UserName"] ?? throw new InvalidOperationException("Email UserName not configured"),
                _configuration["EmailSettings:Password"] ?? throw new InvalidOperationException("Email password not configured"));

            // Send the email
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", email);
            throw new Exception("Error sending email", ex); // Rethrow the exception after logging
        }
        finally
        {
            await client.DisconnectAsync(true); // Ensure to disconnect from the server
        }
    }
    
    public async Task SendConfirmationEmailAsync(string email, string recipientName, string token)
    {
        string subject = "Confirm your email address";

        // Get the base URL from configuration
        string baseUrl = _configuration["ClientAppSettings:BaseUrl"] ?? throw new InvalidOperationException("Client base url not configured");
        
        // Combine the base URL with the token to form the confirmation link
        string confirmationLink = $"{baseUrl}/confirm-email?token={token}&email={email}";

        string messageBody = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Email Confirmation</title>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                .container {{ width: 100%; max-width: 600px; margin: 0 auto; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); }}
                .header {{ background: #007bff; color: #ffffff; padding: 10px 0; text-align: center; border-radius: 8px 8px 0 0; }}
                .content {{ padding: 20px; line-height: 1.6; }}
                .footer {{ text-align: center; padding: 10px 0; font-size: 12px; color: #666; }}
                a {{ color: #007bff; text-decoration: none; }}
                a:hover {{ text-decoration: underline; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Welcome to Our App!</h1>
                </div>
                <div class='content'>
                    <h2>Hello, {recipientName}!</h2>
                    <p>Thank you for signing up. To confirm your email address, please click the link below:</p>
                    <p><a href='{confirmationLink}'>Confirm your email</a></p>
                    <p>Thank you for joining us!</p>
                </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Your App Name. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";

        await SendEmailAsync(email, subject, messageBody);
    }
}