
namespace Mail.Hub.Domain.Reciver
{
    public interface IReviceMailService
    {
        Task ParseNewMails();
    }
}