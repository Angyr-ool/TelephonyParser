using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;

public class BuildPackagesLogic : IBuildPackagesLogic
{
    private static readonly IPackageBuilder[] _packageBuilders = new IPackageBuilder[]
    {
        new FixedPartBuilder(),
        new DateTimeDurationBuilder(),
        new CallingPartyNumberBuilder(),
        new RedirectingNumberBuilder(),
        new ChargeUnitsForConnectionBuilder(),
    };

    public IRecordPackage[] Build(byte[] bytes)
    {
        var packages = new List<IRecordPackage>();
        var packageNumberPosition = 0;

        while (_packageBuilders.FirstOrDefault(x => x.PackageNumber == bytes[packageNumberPosition]) is IPackageBuilder packageBuilder)
        {
            var package = packageBuilder.BuildPackage(bytes, packageNumberPosition);
            packageNumberPosition += package.Length;
            packages.Add(package);
        }

        return packages.ToArray();
    }
}
