using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TelephonyParser.EwsdParser.BusinessLogic;
using TelephonyParser.EwsdParser.BusinessLogic.FilesProcessLogics;

namespace TelephonyParser.EwsdParser.Infrastructure;

/// <summary>
/// Сервис для обработки файлов ewsd
/// </summary>
public class EwsdParserHostedService : IHostedService
{
    private readonly ILogger<EwsdParserHostedService> _logger;
    private readonly IEwsdFilesProcessLogic _filesProcessLogic;

    public EwsdParserHostedService(ILogger<EwsdParserHostedService> logger,
        IHostApplicationLifetime appLifetime, IEwsdFilesProcessLogic filesProcessLogic)
    {
        _logger = logger;
        _filesProcessLogic = filesProcessLogic;

        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync has been called.");

        try
        {
            await _filesProcessLogic.ProcessFilesAsync(cancellationToken);
        }
        catch (TaskCanceledException e)
        {
            _logger.LogWarning(e.Message);
        }
        catch(Exception e)
        {
            _logger.LogCritical(e.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StopAsync has been called.");
        return Task.CompletedTask;
    }

    private void OnStopping()
    {
        _logger.LogInformation("OnStopping has been called.");
    }

    private void OnStopped()
    {
        _logger.LogInformation("OnStopped has been called.");
    }
}
