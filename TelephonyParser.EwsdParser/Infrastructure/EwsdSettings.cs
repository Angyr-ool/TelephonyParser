namespace TelephonyParser.EwsdParser.Infrastructure;

public class EwsdSettings
{
    /// <summary>
    /// Задержка после обработки каждого файла (по-умолчанию 1 секунда)
    /// </summary>
    public int ParsingDelayInSeconds { get; set; } = 1;
    
    /// <summary>
    /// Задержка для случая, когда файла для обработки нет (по-умолчанию 5 секунд)
    /// </summary>
    public int NoMoreFileDelayInSeconds { get; set; } = 5;
}