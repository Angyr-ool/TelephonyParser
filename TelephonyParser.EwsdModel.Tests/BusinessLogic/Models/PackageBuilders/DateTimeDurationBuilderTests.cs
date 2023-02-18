using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class DateTimeDurationBuilderTests
{
    private readonly byte[] _dateTimeDurationBytes = new byte[] { 0x64, 0x15, 0x0b, 0x01, 0x00, 0x06, 0x3b, 0x0d, 0x32, 0x00, 0x00 };

    [TestMethod()]
    public void DateTimeDurationBuilder_GetDateTime_Test()
    {
        var expectedDateTime = new DateTime(2021, 11, 01, 00, 06, 59, 100);
        var actual = DateTimeDurationBuilder.GetDateTime(new byte[] { 0x15, 0x0b, 0x01, 0x00, 0x06, 0x3b }, true, 1);
        Assert.AreEqual(expectedDateTime, actual);
    }

    [TestMethod()]
    public void DateTimeDurationBuilder_GetDuration_Test()
    {
        var actual = DateTimeDurationBuilder.GetDuration(new byte[] { 0x32, 0x00, 0x00 });
        Assert.AreEqual(50, actual);
    }

    [TestMethod()]
    public void DateTimeDurationBuilder_BuildPackage_Test()
    {
        var sut = new DateTimeDurationBuilder();
        var actual = (DateTimeDuration)sut.BuildPackage(_dateTimeDurationBytes, 0);

        Assert.AreEqual(new DateTime(2021, 11, 01, 00, 06, 59, 600), actual.DateTime);
        Assert.AreEqual(true, actual.Flags.F0);
        Assert.AreEqual(6, actual.Flags.TenthsOfSeconds);
        Assert.AreEqual(false, actual.Flags.F6);
        Assert.AreEqual(false, actual.Flags.F7);
        Assert.AreEqual(50, actual.Duration);
    }
}