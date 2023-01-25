using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;
using TelephonyParser.EwsdModel.Settings;

namespace TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;

public class ProcessEwsdFilesLogic : IProcessEwsdFilesLogic
{
    private readonly IEwsdSettings _settings;
    private readonly IEwsdExternalResourceService _ewsdExternalResourceService;
    private readonly IParseEwsdFileLogic _parseFileLogic;

    public ProcessEwsdFilesLogic(IEwsdSettings settings, IEwsdExternalResourceService fileTaskExternalResourceService, 
        IParseEwsdFileLogic parseFileLogic)
    {
        _settings = settings;
        _ewsdExternalResourceService = fileTaskExternalResourceService;
        _parseFileLogic = parseFileLogic;
    }

    public event ProcessFilesNotifyEvent? ProcessFilesNotify;
    public event ProcessFilesStatusChangedEvent? ProcessFilesStatusChanged;

    public void ProcessFiles()
    {
        throw new NotImplementedException();
    }

    public async Task ProcessFilesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // берем новую задачу по обработке ewsd файла
            var fileTask = _ewsdExternalResourceService.GetNew();

            if (fileTask is null)
            {
                ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Information, "Файла для обработки нет"));
                ProcessFilesStatusChanged?.Invoke(ProcessFilesStatus.OnWait);

                // задержка для случая, когда файла для обработки нет
                await Task.Delay(TimeSpan.FromSeconds(_settings.NoMoreFileDelayInSeconds), cancellationToken);
                continue;
            }

            ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Information, $"Старт обработки файла: {fileTask.File?.Path}"));
            ProcessFilesStatusChanged?.Invoke(ProcessFilesStatus.OnProcessFile);

            fileTask.Status = EwsdFileTaskStatus.OnProcess;
            // сохраняем задачу
            _ewsdExternalResourceService.Save(fileTask);

            // обработка задачи (парсинг файла)
            await _parseFileLogic.ParseFileAsync(fileTask, cancellationToken);

            // лог после обработки задачи
            switch (fileTask.Status)
            {
                case EwsdFileTaskStatus.Processed:
                    ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Information, $"Файл '{fileTask.File?.Path}' успешно обработан"));
                    break;
                case EwsdFileTaskStatus.New:
                case EwsdFileTaskStatus.OnProcess:
                    ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Critical, $"Недопустимый статус после обработки '{fileTask.Status}'. Файл '{fileTask.File?.Path}'"));
                    break;
                case EwsdFileTaskStatus.Error:
                    ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Error, $"Ошибка во время обработки файла '{fileTask.File?.Path}'"));
                    break;
                default:
                    ProcessFilesNotify?.Invoke(new ProcessFilesEventArgs(ProcessFilesNotifyType.Critical, $"Незарегистрированный статус после обработки '{fileTask.Status}'. Файл '{fileTask.File?.Path}'"));
                    break;
            }

            // сохраняем задачу
            _ewsdExternalResourceService.Save(fileTask);
            ProcessFilesStatusChanged?.Invoke(ProcessFilesStatus.FileProcessed);

            // задержка после обработки каждого файла
            await Task.Delay(TimeSpan.FromSeconds(_settings.ParsingDelayInSeconds), cancellationToken);
        }
    }
}
