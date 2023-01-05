﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TelephonyParser.EwsdParser.Infrastructure;

/// <summary>
/// Сервис для обработки файлов ewsd
/// </summary>
public class EwsdParserHostedService : IHostedService
{
    private readonly ILogger<EwsdParserHostedService> _logger;
    private readonly IDateTimeContext _dateTimeContext;
    private readonly EwsdSettings _settings;

    public EwsdParserHostedService(ILogger<EwsdParserHostedService> logger, IDateTimeContext dateTimeContext, 
        IHostApplicationLifetime appLifetime, EwsdSettings settings)
    {
        _logger = logger;
        _dateTimeContext = dateTimeContext;
        _settings = settings;

        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync has been called.");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_settings.ParsingDelayInSeconds), cancellationToken);
                _logger.LogInformation($"Parsing ewsd files ...  ({_dateTimeContext.Get()})");
            }
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
