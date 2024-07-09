
namespace Mail.Hub.Domain.Sender
{
    public interface IMailSenderService
    {
        Task SendMailAsync(string receiverMail, string mailTitle, string htmlBody);
        Task SendMailsAsync(IEnumerable<string> to, string mailTitle, string htmlBody, IEnumerable<string>? cc = null, IEnumerable<string>? bcc = null);
    }
}