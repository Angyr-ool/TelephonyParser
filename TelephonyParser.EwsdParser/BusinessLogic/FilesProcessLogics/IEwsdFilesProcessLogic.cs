namespace TelephonyParser.EwsdParser.BusinessLogic.FilesProcessLogics;

/// <summary>
/// Логика обработки ewsd файлов
/// </summary>
public interface IEwsdFilesProcessLogic
{
    /// <summary>
    /// Обработать ewsd файлы
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ProcessFilesAsync(CancellationToken cancellationToken);
}