using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class RedirectingNumberBuilderTests
{
    private readonly byte[] _RedirectingNumberBytes = new byte[] { 0xaa, 0x0a, 0x03, 0x10, 0x0a, 0x39, 0x42, 0x24, 0x40, 0x98 };

    [TestMethod()]
    public void RedirectingNumber_BuildPackage_Test()
    {
        var sut = new RedirectingNumberBuilder();
        var actual = (RedirectingNumber)sut.BuildPackage(_RedirectingNumberBytes, 0);

        Assert.AreEqual(10, actual.Length);
        Assert.AreEqual(0x03, actual.Nadi);
        Assert.AreEqual(0x01, actual.Npi);
        Assert.AreEqual(0x00, actual.PresInd);
        Assert.AreEqual(0x00, actual.Screening);
        Assert.AreEqual(0, actual.LacLength);
        Assert.AreEqual(10, actual.NumberOfDigits);
        Assert.AreEqual("3942244098", actual.PackedDigits);
    }
}