using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;

public interface IBuildEwsdPackagesLogic
{
    IEwsdPackage[] Build(byte[] bytes);
}
