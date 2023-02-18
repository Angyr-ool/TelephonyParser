using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class CalledPartyNumberBuilderTests
{
    private readonly byte[] _calledPartyNumberBytes  = new byte[] { 0xa8, 0x0a, 0x03, 0x10, 0x0a, 0x39, 0x42, 0x23, 0x75, 0x53 };
    private readonly byte[] _calledPartyNumberBytes2 = new byte[] { 0xa8, 0x0a, 0x03, 0x10, 0x0a, 0x39, 0x42, 0x23, 0x43, 0x22 };

    [TestMethod()]
    public void CalledPartyNumberBuilder_BuildPackage_Test()
    {
        var sut = new CalledPartyNumberBuilder();
        var actual = (CalledPartyNumber)sut.BuildPackage(_calledPartyNumberBytes, 0);

        Assert.AreEqual(0x03, actual.Nadi);
        Assert.AreEqual(0x01, actual.Npi);
        Assert.AreEqual(0, actual.LacLength);
        Assert.AreEqual(10, actual.NumberOfDigits);
        Assert.AreEqual("3942237553", actual.PackedDigits);
    }

    [TestMethod()]
    public void CalledPartyNumberBuilder_BuildPackage2_Test()
    {
        var sut = new CalledPartyNumberBuilder();
        var actual = (CalledPartyNumber)sut.BuildPackage(_calledPartyNumberBytes2, 0);

        Assert.AreEqual(0x03, actual.Nadi);
        Assert.AreEqual(0x01, actual.Npi);
        Assert.AreEqual(0, actual.LacLength);
        Assert.AreEqual(10, actual.NumberOfDigits);
        Assert.AreEqual("3942234322", actual.PackedDigits);
    }
}