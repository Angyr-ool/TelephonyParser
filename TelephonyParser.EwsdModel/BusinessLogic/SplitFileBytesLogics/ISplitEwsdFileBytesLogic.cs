using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephonyParser.EwsdModel.BusinessLogic.SplitFileBytesLogics;

/// <summary>
/// Логика разбития байтов файла ewsd на массивы записей
/// </summary>
public interface ISplitEwsdFileBytesLogic
{
    byte[][] SplitBytes(byte[] bytes);
}