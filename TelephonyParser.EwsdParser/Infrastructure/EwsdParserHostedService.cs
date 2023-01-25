using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;

namespace TelephonyParser.EwsdParser.Infrastructure;

/// <summary>
/// Сервис для обработки файлов ewsd
/// </summary>
public class EwsdParserHostedService : IHostedService
{
    private readonly ILogger<EwsdParserHostedService> _logger;
    private readonly IProcessEwsdFilesLogic _processFilesLogic;
    private readonly ILogger<IProcessEwsdFilesLogic> _processFilesLogicLogger;

    public EwsdParserHostedService(ILogger<EwsdParserHostedService> logger,
        IHostApplicationLifetime appLifetime, IProcessEwsdFilesLogic filesProcessLogic, 
        ILogger<IProcessEwsdFilesLogic> processFilesLogicLogger)
    {
        _logger = logger;
        _processFilesLogic = filesProcessLogic;
        _processFilesLogicLogger = processFilesLogicLogger;

        _processFilesLogic.ProcessFilesNotify += _processFilesLogic_ProcessFilesNotify;

        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);
    }

    private void _processFilesLogic_ProcessFilesNotify(ProcessFilesEventArgs eventArgs)
    {
        if (eventArgs.NotifyType == ProcessFilesNotifyType.Information)
        {
            _processFilesLogicLogger.LogInformation(eventArgs.Message);
        }

        if (eventArgs.NotifyType == ProcessFilesNotifyType.Critical)
        {
            _processFilesLogicLogger.LogCritical(eventArgs.Message);
        }

        if (eventArgs.NotifyType == ProcessFilesNotifyType.Error)
        {
            _processFilesLogicLogger.LogError(eventArgs.Message);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync has been called.");

        try
        {
            await _processFilesLogic.ProcessFilesAsync(cancellationToken);
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
