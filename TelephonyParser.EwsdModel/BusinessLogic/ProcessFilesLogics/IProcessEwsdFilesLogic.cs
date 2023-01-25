namespace TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;

/// <summary>
/// Логика обработки ewsd файлов
/// </summary>
public interface IProcessEwsdFilesLogic
{
    event ProcessFilesNotifyEvent ProcessFilesNotify;
    event ProcessFilesStatusChangedEvent ProcessFilesStatusChanged;

    /// <summary>
    /// Обработка ewsd файлов
    /// </summary>
    void ProcessFiles();

    /// <summary>
    /// Обработка ewsd файлов (асинхронно)
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ProcessFilesAsync(CancellationToken cancellationToken);
}

public delegate void ProcessFilesNotifyEvent(ProcessFilesEventArgs eventArgs);

public class ProcessFilesEventArgs : EventArgs
{
    public ProcessFilesEventArgs(ProcessFilesNotifyType notifyType, string message)
    {
        NotifyType = notifyType;
        Message = message;
    }

    public ProcessFilesNotifyType NotifyType { get; }
    public string Message { get; }
}

public enum ProcessFilesNotifyType
{
    Information,
    Critical,
    Error
}

public delegate void ProcessFilesStatusChangedEvent(ProcessFilesStatus processFilesStatus);

public enum ProcessFilesStatus
{
    /// <summary>
    /// Идет обработка файла
    /// </summary>
    OnProcessFile,

    /// <summary>
    /// В ожидании файла для обработки
    /// </summary>
    OnWait,

    /// <summary>
    /// Файл обработан
    /// </summary>
    FileProcessed
}