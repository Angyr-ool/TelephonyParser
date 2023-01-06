using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;

/// <summary>
/// Логика парсинга ewsd файла
/// </summary>
public interface IEwsdFileParsingLogic
{
    Task<EwsdFileTask> ParseFileAsync(EwsdFileTask ewsdFileTask, CancellationToken cancellationToken);
}