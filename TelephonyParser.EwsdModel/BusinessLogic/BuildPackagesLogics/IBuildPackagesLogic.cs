using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;

public interface IBuildPackagesLogic
{
    IRecordPackage[] Build(byte[] bytes);
}
