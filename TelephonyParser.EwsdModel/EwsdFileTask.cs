namespace TelephonyParser.EwsdModel;

/// <summary>
/// Задача по обработке ewsd файла
/// </summary>
public class EwsdFileTask
{
    public EwsdFile? File { get; set; }
    public EwsdFileProcessStatus? Status { get; set; }
    public string? Message { get; set; }
}

public enum EwsdFileProcessStatus
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