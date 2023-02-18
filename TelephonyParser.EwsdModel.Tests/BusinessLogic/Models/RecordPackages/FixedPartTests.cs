/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.RecordPackages;

[TestClass()]
public class FixedPartTests
{
    #region Static Methods

    /// <summary>
    /// Record Length | Field 2
    /// </summary>
    [TestMethod()]
    public void FixedPart_GetRecordLength_Test()
    {
        var recordLenth1 = FixedPart.GetRecordLength(new byte[] { 0x51, 0x00 });
        Assert.AreEqual(81, recordLenth1);

        var recordLenth2 = FixedPart.GetRecordLength(new byte[] { 0x55, 0x00 });
        Assert.AreEqual(85, recordLenth2);
    }

    /// <summary>
    /// Flags From F11 To F18 | Field 3
    /// </summary>
    [TestMethod()]
    public void FixedPart_GetFlagsFromF11ToF18_Test()
    {
        var (F11, F13, F14, F15, F16, F17, F18) = FixedPart.GetFlagsFromF11ToF18(0x8c);

        Assert.AreEqual(false, F11);
        Assert.AreEqual(true, F13);
        Assert.AreEqual(true, F14);
        Assert.AreEqual(false, F15);
        Assert.AreEqual(false, F16);
        Assert.AreEqual(false, F17);
        Assert.AreEqual(true, F18);
    }

    /// <summary>
    /// Flags From F21 To F27 | Field 3
    /// </summary>
    [TestMethod()]
    public void FixedPart_GetFlagsFromF21ToF27_Test()
    {
        var (F21, F22, F23, F24, F25, F27) = FixedPart.GetFlagsFromF21ToF27(0x00);

        Assert.AreEqual(false, F21);
        Assert.AreEqual(false, F22);
        Assert.AreEqual(false, F23);
        Assert.AreEqual(false, F24);
        Assert.AreEqual(false, F25);
        Assert.AreEqual(false, F27);
    }

    /// <summary>
    /// Flags From F33 To F38 | Field 3
    /// </summary>
    [TestMethod()]
    public void FixedPart_GetFlagsFromF33ToF38_Test()
    {
        var (F33, F34, F35, F37, F38) = FixedPart.GetFlagsFromF33ToF38(0x00);

        Assert.AreEqual(false, F33);
        Assert.AreEqual(false, F34);
        Assert.AreEqual(false, F35);
        Assert.AreEqual(false, F37);
        Assert.AreEqual(false, F38);
    }

    /// <summary>
    /// Record Sequence And Charge Status | Field 4
    /// </summary>
    [TestMethod()]
    public void FixedPart_GetRecordSequenceAndChargeStatus_Test()
    {
        var (RecordSequence1, ChargeStatus1) = FixedPart.GetRecordSequenceAndChargeStatus(0x11);
        Assert.AreEqual(1, RecordSequence1);
        Assert.AreEqual(1, ChargeStatus1);

        var (RecordSequence2, ChargeStatus2) = FixedPart.GetRecordSequenceAndChargeStatus(0x11);
        Assert.AreEqual(1, RecordSequence2);
        Assert.AreEqual(1, ChargeStatus2);
    }

    #endregion

    private readonly byte[] _bytes1 = new byte[] { 0x84, 0x51, 0x00, 0x8c, 0x00, 0x00, 0x11, 0x8e };
    private readonly byte[] _bytes2 = new byte[] { 0x84, 0x55, 0x00, 0x8c, 0x10, 0x00, 0x11, 0x8e };

    [TestMethod()]
    public void FixedPart_InitProperties_Test()
    {
        // arrange
        var fixedPartPackage = new FixedPart();
        fixedPartPackage.SetPackageNumberPosition(_bytes1, 0);

        // act
        fixedPartPackage.InitProperties(_bytes1);

        // assert
        Assert.AreEqual(81, fixedPartPackage.RecordLength);

        Assert.AreEqual(false, fixedPartPackage.F11);
        Assert.AreEqual(true, fixedPartPackage.F13);
        Assert.AreEqual(true, fixedPartPackage.F14);
        Assert.AreEqual(false, fixedPartPackage.F15);
        Assert.AreEqual(false, fixedPartPackage.F16);
        Assert.AreEqual(false, fixedPartPackage.F17);
        Assert.AreEqual(true, fixedPartPackage.F18);

        Assert.AreEqual(false, fixedPartPackage.F21);
        Assert.AreEqual(false, fixedPartPackage.F22);
        Assert.AreEqual(false, fixedPartPackage.F23);
        Assert.AreEqual(false, fixedPartPackage.F24);
        Assert.AreEqual(false, fixedPartPackage.F25);
        Assert.AreEqual(false, fixedPartPackage.F27);

        Assert.AreEqual(false, fixedPartPackage.F33);
        Assert.AreEqual(false, fixedPartPackage.F34);
        Assert.AreEqual(false, fixedPartPackage.F35);
        Assert.AreEqual(false, fixedPartPackage.F37);
        Assert.AreEqual(false, fixedPartPackage.F38);

        Assert.AreEqual((byte)0x01, fixedPartPackage.RecordSequence);
        Assert.AreEqual((byte)0x01, fixedPartPackage.ChargeStatus);

        Assert.AreEqual((byte)0x8e, fixedPartPackage.RecordOwnerType);
    }
}*/