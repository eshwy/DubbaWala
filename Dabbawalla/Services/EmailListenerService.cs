using Dabbawalla.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Dabbawalla.Services
{
    public class EmailListenerService
    {
        private readonly IConfiguration _configuration;
        public EmailListenerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAutoReplyAsync(string toEmail, string subject, string userId, string password, string ticketId, CancellationToken cancellationToken)
        {
            var imapSettingsLocal = _configuration.GetSection("ImapSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DUBBAWALLA", imapSettingsLocal["Email"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Re: {subject}";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <!DOCTYPE html>
                    <html>
                    <body>
                        <h1>Hello!</h1>
                        <p>Your ticket has been received and assigned.</p>
                        <p><strong>User ID:</strong> {userId}</p>
                        <p><strong>Temporary Password:</strong> {password}</p>
                        <p><strong>Ticket ID:</strong> {ticketId}</p>
                    </body>
                    </html>"
                        };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                await smtpClient.AuthenticateAsync(imapSettingsLocal["Email"], imapSettingsLocal["Password"], cancellationToken);
                await smtpClient.SendAsync(message, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);
            }
        }

        public async Task SendForgotPassword(string toEmail, string password, CancellationToken cancellationToken)
        {
            var imapSettingsLocal = _configuration.GetSection("ImapSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DUBBAWALLA", imapSettingsLocal["Email"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"🔐 Password Reset - Dubbawalla";

            message.Body = new TextPart("html")
            {
                Text = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 40px auto;
                    background-color: #ffffff;
                    padding: 30px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0,0,0,0.05);
                }}
                h1 {{
                    color: #264653;
                    font-size: 24px;
                }}
                p {{
                    font-size: 16px;
                    color: #333333;
                    line-height: 1.6;
                }}
                .details {{
                    margin-top: 20px;
                    padding: 20px;
                    background-color: #f0f0f0;
                    border-radius: 8px;
                }}
                .details p {{
                    margin: 8px 0;
                    font-weight: 500;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 13px;
                    color: #888888;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Password Reset Successful 🔐</h1>
                <p>Hello,</p>
                <p>Your password has been successfully reset. Please find your new credentials below:</p>
                <div class='details'>
                    <p><strong>📧 Email:</strong> {toEmail}</p>
                    <p><strong>🔑 New Password:</strong> {password}</p>
                </div>
                <p>We recommend changing your password after login to something memorable and secure.</p>
                <div class='footer'>
                    <p>&copy; 2025 Dubbawalla | For Help, Contact support@dubbawalla.in</p>
                </div>
            </div>
        </body>
        </html>"
            };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                await smtpClient.AuthenticateAsync(imapSettingsLocal["Email"], imapSettingsLocal["Password"], cancellationToken);
                await smtpClient.SendAsync(message, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);
            }
        }


        public async Task SendVendorMail(string toEmail,string name, string foodItemName, int quaity,string address, CancellationToken cancellationToken)
        {
            var imapSettingsLocal = _configuration.GetSection("ImapSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DUBBAWALLA", imapSettingsLocal["Email"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Order Delivery Details";

            message.Body = new TextPart("html")
            {
                Text = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 40px auto;
                            background-color: #ffffff;
                            padding: 30px;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0,0,0,0.05);
                        }}
                        h1 {{
                            color: #e76f51;
                            margin-bottom: 20px;
                        }}
                        p {{
                            font-size: 16px;
                            color: #333333;
                            line-height: 1.6;
                        }}
                        .details {{
                            margin-top: 20px;
                            padding: 20px;
                            background-color: #f0f0f0;
                            border-radius: 8px;
                        }}
                        .details p {{
                            margin: 8px 0;
                            font-weight: 500;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 14px;
                            color: #777777;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Hi {name},</h1>
                        <p><strong>You have received a new order!</strong></p>
                        <div class='details'>
                            <p><strong>🍱 Food Item:</strong> {foodItemName}</p>
                            <p><strong>🔢 Quantity:</strong> {quaity}</p>
                            <p><strong>📍 Delivery Address:</strong><br>{address}</p>
                        </div>
                        <p>Kindly prepare and deliver the meal promptly. Thank you for your service!</p>
                        <div class='footer'>
                            <p>&copy; 2025 Dubbawalla | Connecting Homes with Homely Meals</p>
                        </div>
                    </div>
                </body>
                </html>"
                        };


                        using (var smtpClient = new SmtpClient())
                        {
                            await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                            await smtpClient.AuthenticateAsync(imapSettingsLocal["Email"], imapSettingsLocal["Password"], cancellationToken);
                            await smtpClient.SendAsync(message, cancellationToken);
                            await smtpClient.DisconnectAsync(true, cancellationToken);
                        }
                    }
        public async Task SendCoustomerMail(string toEmail, string name, string foodItemName, int quaity, decimal price, CancellationToken cancellationToken)
        {
            var imapSettingsLocal = _configuration.GetSection("ImapSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DUBBAWALLA", imapSettingsLocal["Email"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Your Order Confirmation - Dubbawalla";

            message.Body = new TextPart("html")
            {
                Text = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    background-color: #f9f9f9;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 40px auto;
                    background-color: #ffffff;
                    padding: 30px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0,0,0,0.05);
                }}
                h1 {{
                    color: #2a9d8f;
                    margin-bottom: 20px;
                }}
                p {{
                    font-size: 16px;
                    color: #333333;
                    line-height: 1.6;
                }}
                .details {{
                    margin-top: 20px;
                    padding: 20px;
                    background-color: #f0f0f0;
                    border-radius: 8px;
                }}
                .details p {{
                    margin: 8px 0;
                    font-weight: 500;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 14px;
                    color: #777777;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Hello {name},</h1>
                <p><strong>Thank you for your order with Dubbawalla!</strong></p>
                <p>We're excited to deliver your delicious meal soon. Here are the details of your order:</p>
                <div class='details'>
                    <p><strong>🍽️ Food Item:</strong> {foodItemName}</p>
                    <p><strong>🔢 Quantity:</strong> {quaity}</p>
                    <p><strong>💰 Total Cost:</strong> ₹{price}</p>
                </div>
                <p>If you have any questions or need to modify your order, please contact our support team.</p>
                <p>We appreciate your trust in us. Enjoy your meal!</p>
                <div class='footer'>
                    <p>&copy; 2025 Dubbawalla | Warm Meals, Warm Hearts</p>
                </div>
            </div>
        </body>
        </html>"
            };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                await smtpClient.AuthenticateAsync(imapSettingsLocal["Email"], imapSettingsLocal["Password"], cancellationToken);
                await smtpClient.SendAsync(message, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);
            }
        }

        public async Task SendAdminMailDForContact(List<string> emails, ContactUs contact, CancellationToken cancellationToken)
        {
            var imapSettingsLocal = _configuration.GetSection("ImapSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DUBBAWALLA", imapSettingsLocal["Email"]));

            // Add all admin recipients
            foreach (var email in emails.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                message.To.Add(MailboxAddress.Parse(email));
            }

            message.Subject = $"📩 New Contact Message from {contact.UserName ?? "User"} - Dubbawalla";

            message.Body = new TextPart("html")
            {
                Text = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f6f8fa;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #2a9d8f;
            margin-bottom: 20px;
        }}
        .details {{
            background-color: #f0f4f8;
            padding: 20px;
            border-radius: 8px;
        }}
        .details p {{
            margin: 10px 0;
            font-size: 16px;
            color: #333;
        }}
        .footer {{
            margin-top: 30px;
            text-align: center;
            font-size: 14px;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>📬 You've received a new contact message!</h2>
        <div class='details'>
            <p><strong>👤 Name:</strong> {contact.UserName ?? "N/A"}</p>
            <p><strong>📧 Email:</strong> {contact.Email ?? "N/A"}</p>
            <p><strong>📝 Subject:</strong> {contact.Subject ?? "No Subject"}</p>
            <p><strong>💬 Message:</strong><br />{contact.Message?.Replace("\n", "<br>") ?? "No message provided."}</p>
        </div>
        <div class='footer'>
            <p>&copy; 2025 Dubbawalla | Customer Contact Notification</p>
        </div>
    </div>
</body>
</html>"
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            await smtpClient.AuthenticateAsync(imapSettingsLocal["Email"], imapSettingsLocal["Password"], cancellationToken);
            await smtpClient.SendAsync(message, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
        }


    }
}
