namespace TelephonyParser.EwsdParser.BusinessLogic;

/// <summary>
/// Логика разбития байтов файла ewsd на массивы записей
/// </summary>
public interface IEwsdFileBytesSplitLogic
{
    byte[][] SplitBytes(byte[] bytes);
}
