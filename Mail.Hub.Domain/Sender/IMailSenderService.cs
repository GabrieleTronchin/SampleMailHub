namespace Mail.Hub.Domain.Sender
{
    public interface IMailSenderService
    {
        Task SendMail(string body);
    }
}
