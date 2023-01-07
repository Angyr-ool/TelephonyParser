using Microsoft.Extensions.Logging;
using TelephonyParser.EwsdModel;
using TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;
using TelephonyParser.EwsdParser.Infrastructure;

namespace TelephonyParser.EwsdParser.BusinessLogic.FilesProcessLogics;

public class EwsdFilesProcessLogic : IEwsdFilesProcessLogic
{
    private readonly EwsdSettings _settings;
    private readonly ILogger<EwsdFilesProcessLogic> _logger;
    private readonly IEwsdFileParsingLogic _fileParsingLogic;
    private readonly IProcessEwsdFileTaskManager _processFileTaskManager;

    public EwsdFilesProcessLogic(EwsdSettings settings, 
        ILogger<EwsdFilesProcessLogic> logger, IEwsdFileParsingLogic fileParsingLogic, 
        IProcessEwsdFileTaskManager processFileTaskManager)
    {
        _settings = settings;
        _logger = logger;
        _fileParsingLogic = fileParsingLogic;
        _processFileTaskManager = processFileTaskManager;
    }
    
    /// <summary>
    /// Обработать ewsd файлы асинхронно
    /// </summary>
    /// <param name="cancellationToken">токен отмены</param>
    public async Task ProcessFilesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Ewsd files process started");

        while (!cancellationToken.IsCancellationRequested)
        {
            // берем новую задачу по обработке ewsd файла
            var fileProcessTask = _processFileTaskManager.GetNew();
            
            if (fileProcessTask is null)
            {
                // задержка для случая, когда файла для обработки нет
                await Task.Delay(TimeSpan.FromSeconds(_settings.NoMoreFileDelayInSeconds), cancellationToken);
                continue;
            }
            
            // обработка задачи
            await _fileParsingLogic.ParseFileAsync(fileProcessTask, cancellationToken);

            // лог после обработки задачи
            switch (fileProcessTask.Status)
            {
                case EwsdFileProcessStatus.Processed:
                    _logger.LogInformation($"Файл '{fileProcessTask.File?.Path}' успешно обработан");
                    break;
                case EwsdFileProcessStatus.New:
                case EwsdFileProcessStatus.OnProcess:
                    _logger.LogCritical($"Недопустимый статус после обработки '{fileProcessTask.Status}'. " +
                                        $"Файл '{fileProcessTask.File?.Path}'");
                    break;
                case EwsdFileProcessStatus.Error:
                    _logger.LogError($"Ошибка во время обработки файла '{fileProcessTask.File?.Path}'");
                    break;
                default:
                    _logger.LogCritical($"Незарегистрированный статус после обработки '{fileProcessTask.Status}'. " +
                                        $"Файл '{fileProcessTask.File?.Path}'");
                    break;
            }
            
            // сохраняем задачу
            _processFileTaskManager.Save(fileProcessTask);
            
            // задержка после обработки каждого файла
            await Task.Delay(TimeSpan.FromSeconds(_settings.ParsingDelayInSeconds), cancellationToken);
        }
    }
}