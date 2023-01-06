using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic;

public interface IProcessEwsdFileTaskManager
{
    /// <summary>
    /// Вернуть новую задачу по обработке ewsd файла
    /// </summary>
    /// <returns></returns>
    EwsdFileTask? GetNew();

    /// <summary>
    /// Сохранить задачу
    /// </summary>
    /// <param name="fileTask"></param>
    void Save(EwsdFileTask fileTask);
}