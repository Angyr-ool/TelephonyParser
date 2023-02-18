using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.PackageBuilders;

[TestClass()]
public class FixedPartBuilderTests
{
    [TestMethod()]
    public void FixedPartBuilder_GetRecordLength_Test()
    {
        var recordLenth1 = FixedPartBuilder.GetRecordLength(new byte[] { 0x51, 0x00 });
        Assert.AreEqual(81, recordLenth1);

        var recordLenth2 = FixedPartBuilder.GetRecordLength(new byte[] { 0x55, 0x00 });
        Assert.AreEqual(85, recordLenth2);
    }

    [TestMethod()]
    public void FixedPartBuilder_GetRecordSequenceAndChargeStatus_Test()
    {
        var (RecordSequence1, ChargeStatus1) = FixedPartBuilder.GetRecordSequenceAndChargeStatus(0x11);
        Assert.AreEqual(1, RecordSequence1);
        Assert.AreEqual(1, ChargeStatus1);

        var (RecordSequence2, ChargeStatus2) = FixedPartBuilder.GetRecordSequenceAndChargeStatus(0x11);
        Assert.AreEqual(1, RecordSequence2);
        Assert.AreEqual(1, ChargeStatus2);
    }

    private readonly byte[] _fixedPartBytes  = new byte[] { 0x84, 0x51, 0x00, 0x8c, 0x00, 0x00, 0x11, 0x8e };
    private readonly byte[] _fixedPartBytes2 = new byte[] { 0x84, 0x51, 0x00, 0x8c, 0x00, 0x00, 0x11, 0xaa };

    [TestMethod()]
    public void FixedPartBuilder_BuildPackage_Test()
    {
        var sut = new FixedPartBuilder();
        var actual = (FixedPart)sut.BuildPackage(_fixedPartBytes, 0);

        Assert.AreEqual(81, actual.RecordLength);

        Assert.AreEqual(false, actual.Flags.F11);
        Assert.AreEqual(true, actual.Flags.F13);
        Assert.AreEqual(true, actual.Flags.F14);
        Assert.AreEqual(false, actual.Flags.F15);
        Assert.AreEqual(false, actual.Flags.F16);
        Assert.AreEqual(false, actual.Flags.F17);
        Assert.AreEqual(true, actual.Flags.F18);

        Assert.AreEqual(false, actual.Flags.F21);
        Assert.AreEqual(false, actual.Flags.F22);
        Assert.AreEqual(false, actual.Flags.F23);
        Assert.AreEqual(false, actual.Flags.F24);
        Assert.AreEqual(false, actual.Flags.F25);
        Assert.AreEqual(false, actual.Flags.F27);

        Assert.AreEqual(false, actual.Flags.F33);
        Assert.AreEqual(false, actual.Flags.F34);
        Assert.AreEqual(false, actual.Flags.F35);
        Assert.AreEqual(false, actual.Flags.F37);
        Assert.AreEqual(false, actual.Flags.F38);

        Assert.AreEqual((byte)0x01, actual.RecordSequence);
        Assert.AreEqual((byte)0x01, actual.ChargeStatus);

        Assert.AreEqual((byte)0x8e, actual.RecordOwnerType);
    }

    [TestMethod()]
    public void FixedPartBuilder_BuildPackage2_Test()
    {
        var sut = new FixedPartBuilder();
        var actual = (FixedPart)sut.BuildPackage(_fixedPartBytes2, 0);

        Assert.AreEqual(81, actual.RecordLength);

        Assert.AreEqual(false, actual.Flags.F11);
        Assert.AreEqual(true, actual.Flags.F13);
        Assert.AreEqual(true, actual.Flags.F14);
        Assert.AreEqual(false, actual.Flags.F15);
        Assert.AreEqual(false, actual.Flags.F16);
        Assert.AreEqual(false, actual.Flags.F17);
        Assert.AreEqual(true, actual.Flags.F18);

        Assert.AreEqual(false, actual.Flags.F21);
        Assert.AreEqual(false, actual.Flags.F22);
        Assert.AreEqual(false, actual.Flags.F23);
        Assert.AreEqual(false, actual.Flags.F24);
        Assert.AreEqual(false, actual.Flags.F25);
        Assert.AreEqual(false, actual.Flags.F27);

        Assert.AreEqual(false, actual.Flags.F33);
        Assert.AreEqual(false, actual.Flags.F34);
        Assert.AreEqual(false, actual.Flags.F35);
        Assert.AreEqual(false, actual.Flags.F37);
        Assert.AreEqual(false, actual.Flags.F38);

        Assert.AreEqual((byte)0x01, actual.RecordSequence);
        Assert.AreEqual((byte)0x01, actual.ChargeStatus);

        Assert.AreEqual((byte)0xAA, actual.RecordOwnerType);
    }
}