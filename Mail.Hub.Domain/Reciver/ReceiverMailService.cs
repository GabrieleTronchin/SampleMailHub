using Mail.Hub.Domain.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mail.Hub.Domain.Reciver;

public class ReceiverMailService : IReceiverMailService
{
    private readonly ILogger<ReceiverMailService> _logger;
    private readonly ReceiverMailOptions _options;
    private readonly IMediator _mediator;

    public ReceiverMailService(
        ILogger<ReceiverMailService> logger,
        IOptions<ReceiverMailOptions> options,
        IMediator mediator
    )
    {
        _logger = logger;
        _options = options.Value;
        _mediator = mediator;
    }

    public async Task ParseNewMails()
    {
        try
        {
            using var client = new ImapClient();
            client.Connect(_options.Server, _options.Port, false);
            client.Authenticate(_options.UserName, _options.Password);
            _logger.LogInformation("Connected");

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);

            _logger.LogInformation("Total messages: {0}", inbox.Count);
            _logger.LogInformation("Recent messages: {0}", inbox.Recent);

            var query = SearchQuery.Not(SearchQuery.Seen);

            var messages = await inbox.SearchAsync(query);

            foreach (var item in messages)
            {
                var message = await inbox.GetMessageAsync(item);

                await _mediator.Send(
                    new NewMailCommand() { Title = message.Subject, HtmlBody = message.HtmlBody }
                );

                inbox.AddFlags(item, MessageFlags.Seen, true);
            }

            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Not work");
            throw;
        }
    }
}
