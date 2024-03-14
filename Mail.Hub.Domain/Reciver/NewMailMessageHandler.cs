using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Hub.Domain.Reciver
{
    public class NewMailMessageHandler : IRequestHandler<NewMailCommand>
    {
        private readonly ILogger<NewMailMessageHandler> _logger;

        public NewMailMessageHandler(ILogger<NewMailMessageHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(NewMailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{request.Title} - {request.HtmlBody}");
        }
    }
}
