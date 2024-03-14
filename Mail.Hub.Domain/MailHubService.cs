using Mail.Hub.Domain.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Mail.Hub.Domain
{
    public class MailHubService(ILogger<MailHubService> logger, IOptions<SenderMailOptions> options)
    {

        public async Task<List<string>> ReciveMails()
        {

            try
            {
                using var client = new ImapClient();
                client.Connect("localhost", 3143, false);
                client.Authenticate("mytestmail@test.it", "test");
                logger.LogInformation("Connected");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                logger.LogInformation("Total messages: {0}", inbox.Count);
                logger.LogInformation("Recent messages: {0}", inbox.Recent);

                List<string> titles = new();

                var query = SearchQuery.New;

                var messages = await inbox.SearchAsync(query);

                foreach (var item in messages)
                {
                    var message = await inbox.GetMessageAsync(item);
                    titles.Add(message.Subject);
                }

                client.Disconnect(true);

                return titles;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Not work");
                throw;
            }0
        }


        public async Task SendMail()
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("FromName", "fromAddress@gmail.com"));
                message.To.Add(new MailboxAddress("test", "mytestmail@test.it"));
                message.Subject = "test";
                message.Body = new TextPart("plain") { Text = "test" };

                using var client = new SmtpClient();
                client.Connect("localhost", 3025, false);
                client.Authenticate("test", "test");
                client.Send(message);
                client.Disconnect(true);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Not work");
                throw;
            }
        }



    }
}
