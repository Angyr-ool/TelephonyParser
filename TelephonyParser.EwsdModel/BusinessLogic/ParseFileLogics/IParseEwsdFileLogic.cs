using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;

/// <summary>
/// Логика парсинга ewsd файла
/// </summary>
public interface IParseEwsdFileLogic
{
    /// <summary>
    /// Парсинг ewsd файла
    /// </summary>
    /// <param name="ewsdFileTask">Задача по обработке ewsd файла</param>
    void ParseFile(IEwsdFileTask ewsdFileTask);

    /// <summary>
    /// Парсинг ewsd файла (асинхронно)
    /// </summary>
    /// <param name="ewsdFileTask">Парсинг ewsd файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ParseFileAsync(IEwsdFileTask ewsdFileTask, CancellationToken cancellationToken);
}
