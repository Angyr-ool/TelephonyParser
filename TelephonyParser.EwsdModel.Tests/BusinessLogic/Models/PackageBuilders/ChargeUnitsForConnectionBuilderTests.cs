using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class ChargeUnitsForConnectionBuilderTests
{
    [TestMethod()]
    public void ChargeUnitsForConnectionBuilder_GetCallChargeUnits_Test()
    {
        var actual = ChargeUnitsForConnectionBuilder.GetCallChargeUnits(new byte[] { 0x00, 0x00, 0x00 });
        Assert.AreEqual(0, actual);
    }

    [TestMethod()]
    public void ChargeUnitsForConnectionBuilder_BuildPackage_Test()
    {
        var sut = new ChargeUnitsForConnectionBuilder();
        var actual = (ChargeUnitsForConnection)sut.BuildPackage(new byte[] { 0x67, 0xff, 0xff, 0x00 }, 0);
        Assert.AreEqual(65535, actual.CallChargeUnits);
    }
}