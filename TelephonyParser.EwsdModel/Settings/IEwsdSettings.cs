using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephonyParser.EwsdModel.Settings;

public interface IEwsdSettings
{
    /// <summary>
    /// Задержка после обработки каждого файла (по-умолчанию 1 секунда)
    /// </summary>
    int ParsingDelayInSeconds { get; set; }

    /// <summary>
    /// Задержка для случая, когда файла для обработки нет (по-умолчанию 5 секунд)
    /// </summary>
    int NoMoreFileDelayInSeconds { get; set; }
}
