using Microsoft.Extensions.Logging;
using Quartz;

namespace Mail.Hub.Domain.Reciver;

public class IncomeMailsJob : IJob
{
    private readonly ILogger<IncomeMailsJob> _logger;
    private readonly IReviceMailService _reviceMailService;

    public IncomeMailsJob(ILogger<IncomeMailsJob> logger, IReviceMailService reviceMailService)
    {
        _logger = logger;
        _reviceMailService = reviceMailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"{nameof(IncomeMailsJob)} - Execution Start");

        try
        {
            await _reviceMailService.ParseNewMails();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{nameof(IncomeMailsJob)} - Execution Stop");
        }

        _logger.LogInformation($"{nameof(IncomeMailsJob)} - Execution Stop");

    }
}
