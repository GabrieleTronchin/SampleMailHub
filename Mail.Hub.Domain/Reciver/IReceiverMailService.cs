
namespace Mail.Hub.Domain.Reciver;

public interface IReceiverMailService
{
    Task ParseNewMails();
}