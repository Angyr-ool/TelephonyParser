using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephonyParser.EwsdModel.Settings;

public class EwsdSettings : IEwsdSettings
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
