using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;

/// <summary>
/// Логика парсинга ewsd файла
/// </summary>
public interface IEwsdFileParsingLogic
{
    void ParseFile(EwsdFileTask ewsdFileTask);
    Task ParseFileAsync(EwsdFileTask ewsdFileTask, CancellationToken cancellationToken);
}