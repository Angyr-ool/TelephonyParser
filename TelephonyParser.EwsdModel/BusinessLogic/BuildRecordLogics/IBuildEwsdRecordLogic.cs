using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;

/// <summary>
/// Логика построения записи ewsd из пакетов ewsd
/// </summary>
public interface IBuildEwsdRecordLogic
{
    IEwsdRecord? Build(IEwsdPackage[] packages);
}
