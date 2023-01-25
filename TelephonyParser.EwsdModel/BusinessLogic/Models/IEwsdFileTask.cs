namespace TelephonyParser.EwsdModel.BusinessLogic.Models;

/// <summary>
/// Задача по обработке ewsd файла
/// </summary>
public interface IEwsdFileTask
{
    IEwsdFile? File { get; set; }
    EwsdFileTaskStatus? Status { get; set; }
    string? Message { get; set; }
}

public enum EwsdFileTaskStatus
{
    /// <summary>
    /// Новая задача
    /// </summary>
    New = 1,

    /// <summary>
    /// Задача в обработке
    /// </summary>
    OnProcess,

    /// <summary>
    /// Задача обработана
    /// </summary>
    Processed,

    /// <summary>
    /// Возникла ошибка при обработке задачи
    /// </summary>
    Error
}
