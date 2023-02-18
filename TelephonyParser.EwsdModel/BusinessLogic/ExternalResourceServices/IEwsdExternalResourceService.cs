using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;

public interface IEwsdExternalResourceService
{
    /// <summary>
    /// Вернуть задачу для обработки
    /// </summary>
    /// <returns></returns>
    IEwsdFileTask? GetNew();

    /// <summary>
    /// Вернуть задачу для обработки (асинхронно)
    /// </summary>
    /// <returns></returns>
    Task<IEwsdFileTask?> GetNewAsync();

    void Save(IEwsdFileTask ewsdFileTask);

    Task SaveAsync(IEwsdFileTask ewsdFileTask, CancellationToken cancellationToken);

    void Save(IRecord[] records);
}
