using MediatR;

namespace Mail.Hub.Domain.Reciver
{
    public class NewMailCommand : IRequest
    {

        public string Title { get; set; }

        public string HtmlBody { get; set; }

    }
}
