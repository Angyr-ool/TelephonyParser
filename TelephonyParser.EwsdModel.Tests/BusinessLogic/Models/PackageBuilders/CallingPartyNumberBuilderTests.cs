using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class CallingPartyNumberBuilderTests
{
    private readonly byte[] _callingPartyNumberBytes = new byte[] { 0x8e, 0x0a, 0x03, 0x13, 0x0a, 0x98, 0x35, 0x91, 0x36, 0x28 };

    [TestMethod()]
    public void CallingPartyNumberBuilder_BuildPackage_Test()
    {
        var sut = new CallingPartyNumberBuilder();
        var actual = (CallingPartyNumber)sut.BuildPackage(_callingPartyNumberBytes, 0);

        Assert.AreEqual(10, actual.Length);
        Assert.AreEqual(0x03, actual.Nadi);
        Assert.AreEqual(0x01, actual.Npi);
        Assert.AreEqual(0x00, actual.PresInd);
        Assert.AreEqual(0x03, actual.Screening);
        Assert.AreEqual(0, actual.LacLength);
        Assert.AreEqual(10, actual.NumberOfDigits);
        Assert.AreEqual("9835913628", actual.PackedDigits);
    }
}