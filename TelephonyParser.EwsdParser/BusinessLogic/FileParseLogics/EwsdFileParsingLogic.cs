using Microsoft.Extensions.Logging;
using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;

public class EwsdFileParsingLogic : IEwsdFileParsingLogic
{
    private readonly ILogger<EwsdFileParsingLogic> _logger;

    public EwsdFileParsingLogic(ILogger<EwsdFileParsingLogic> logger)
    {
        _logger = logger;
    }

    public async Task<EwsdFileTask> ParseFileAsync(EwsdFileTask ewsdFileTask, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Parsing ewsd file ...");
        await Task.Delay(1000, cancellationToken);

        //var ewsdFileParsingTask = _ewsdFileParsingTaskManager.GetNew();

        return new EwsdFileTask();
    }
}