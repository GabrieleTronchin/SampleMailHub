using Mail.Hub.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Mail.Hub.Domain.Sender;

public class MailSenderService(
    ILogger<MailSenderService> logger,
    IOptions<SenderMailOptions> options
) : IMailSenderService
{
    public async Task SendMail(string body)
    {
        try
        {
            //TODO Map options
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FromName", "fromAddress@gmail.com"));
            message.To.Add(new MailboxAddress("test", "mytestmail@test.it"));
            message.Subject = "test";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<b>{body}</b>" };

            using var client = new SmtpClient();
            client.Connect("localhost", 3025, false);
            client.Authenticate("test", "test");
            await client.SendAsync(message);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Not work");
            throw;
        }
    }
}
