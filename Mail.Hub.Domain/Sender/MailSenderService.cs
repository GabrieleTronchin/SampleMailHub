using Mail.Hub.Domain.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Polly.Retry;
using System.Text.RegularExpressions;

namespace Mail.Hub.Domain.Sender;


public class MailSenderService : IMailSenderService
{
    private readonly SenderMailOptions _options;
    private readonly ILogger<MailSenderService> _log;

    const string EMAILPATTERN = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public MailSenderService(IOptions<SenderMailOptions> options, ILogger<MailSenderService> log)
    {
        _options = options.Value;
        _log = log;
    }

    public async Task SendMailAsync(string receiverMail, string mailTitle, string htmlBody)
    {
        ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
            .AddRetry(
                new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder()
                        .Handle<OperationCanceledException>()
                        .Handle<IOException>()
                        .Handle<MailKit.CommandException>()
                        .Handle<MailKit.ProtocolException>(),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(3),
                }
            )
            .AddTimeout(TimeSpan.FromSeconds(10))
            .Build();

        await pipeline.ExecuteAsync(async token =>
            await SendEmailAsync([receiverMail], mailTitle, htmlBody)
        );
    }

    public async Task SendMailsAsync(
        IEnumerable<string> to,
        string mailTitle,
        string htmlBody,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null
    )
    {
        ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
            .AddRetry(
                new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder()
                        .Handle<OperationCanceledException>()
                        .Handle<IOException>()
                        .Handle<MailKit.CommandException>()
                        .Handle<MailKit.ProtocolException>(),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(3),
                }
            )
            .AddTimeout(TimeSpan.FromSeconds(10))
            .Build();

        await pipeline.ExecuteAsync(async token =>
            await SendEmailAsync(to, mailTitle, htmlBody, cc, bcc)
        );
    }


    private async Task SendEmailAsync(
        IEnumerable<string> receiversMail,
        string mailTitle,
        string htmlBody,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null
    )
    {
        IEnumerable<string> mails = receiversMail
            .Union(cc ?? Enumerable.Empty<string>())
            .Union(bcc ?? Enumerable.Empty<string>());
        foreach (var mail in mails)
        {
            if (!IsValidMail(mail))
                throw new ArgumentException($"{mail} is not a valid email");
        }


        cc ??= new List<string>();
        bcc ??= new List<string>();

        var emailMessage = new MimeMessage();
        _log.LogInformation(
            "Sending mail to {email} from {senderMail}({senderName}) -> {smtp}:{port} {user}:{pass}",
            string.Join(";", receiversMail),
            _options.SenderMail,
            _options.SenderName,
            _options.Server,
            _options.Port,
            _options.SenderUsername,
            _options.SenderPassword.Substring(0, 3)
        );

        emailMessage.From.Add(new MailboxAddress(_options.SenderName, _options.SenderMail));
        foreach (var receiverMail in receiversMail)
        {
            emailMessage.To.Add(new MailboxAddress("", receiverMail));
        }

        foreach (var ccMail in cc)
        {
            emailMessage.Cc.Add(new MailboxAddress("", ccMail));
        }

        emailMessage.Subject = mailTitle;
        emailMessage.Body = new TextPart("html") { Text = htmlBody };

        try
        {
            var client = new SmtpClient();

            await client.ConnectAsync(
                                 _options.Server,
                                 _options.Port,
                                 SecureSocketOptions.None
                             );


            await client.AuthenticateAsync(
                _options.SenderUsername,
                _options.SenderPassword
            );

            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _log.LogError(
                ex,
                $"An error occurred sending mail to {string.Join(";", receiversMail)}"
            );
            throw;
        }
    }

    private bool IsValidMail(string email)
    {
        Regex emailRegex = new Regex(EMAILPATTERN);
        return emailRegex.IsMatch(email);
    }
}
