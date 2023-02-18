/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.RecordPackages;

[TestClass()]
public class CallingPartyNumberTests
{
    private readonly byte[] _bytes1 = new byte[] {       0x8e, 0x0a, 0x03, 0x13, 0x0a, 0x98, 0x35, 0x91, 0x38, 0x48 };
    private readonly byte[] _bytes2 = new byte[] { 0x8e, 0x8e, 0x0a, 0x03, 0x13, 0x0a, 0x98, 0x35, 0x91, 0x38, 0x48 };

    [TestMethod()]
    public void CallingPartyNumber_SetPackageNumberPosition_Bytes1_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();

        // act
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes1, 0);

        // assert
        Assert.AreEqual(0, callingPartyNumberPackage.PackageNumberPosition);
    }

    [TestMethod()]
    public void CallingPartyNumber_SetPackageNumberPosition_Bytes2_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();

        // act
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes2, 0);

        // assert
        Assert.AreEqual(1, callingPartyNumberPackage.PackageNumberPosition);
    }

    [TestMethod()]
    public void CallingPartyNumber_GetLength_Bytes1_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes1, 0);

        // act
        var actualLength = callingPartyNumberPackage.GetLength(_bytes1);

        // assert
        Assert.AreEqual(10, actualLength);
    }

    [TestMethod()]
    public void CallingPartyNumber_GetLength_Bytes2_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes2, 0);

        // act
        var actualLength = callingPartyNumberPackage.GetLength(_bytes2);

        // assert
        Assert.AreEqual(10, actualLength);
    }

    [TestMethod()]
    public void CallingPartyNumber_InitProperties_Bytes1_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes1, 0);

        // act
        callingPartyNumberPackage.InitProperties(_bytes1);

        // assert
        Assert.AreEqual(142, callingPartyNumberPackage.Number);
        Assert.AreEqual((byte)3, callingPartyNumberPackage.Nadi);
        Assert.AreEqual((byte)1, callingPartyNumberPackage.Npi);
        Assert.AreEqual((byte)0, callingPartyNumberPackage.PresInd);
        Assert.AreEqual((byte)3, callingPartyNumberPackage.Screening);
        Assert.AreEqual(0, callingPartyNumberPackage.LacLength);
        Assert.AreEqual(10, callingPartyNumberPackage.NumberOfDigits);
        Assert.AreEqual("9835913848", callingPartyNumberPackage.Value);
    }

    [TestMethod()]
    public void CallingPartyNumber_InitProperties_Bytes2_Test()
    {
        // arrange
        var callingPartyNumberPackage = new CallingPartyNumber();
        callingPartyNumberPackage.SetPackageNumberPosition(_bytes2, 0);

        // act
        callingPartyNumberPackage.InitProperties(_bytes2);

        // assert
        Assert.AreEqual(142, callingPartyNumberPackage.Number);
        Assert.AreEqual((byte)3, callingPartyNumberPackage.Nadi);
        Assert.AreEqual((byte)1, callingPartyNumberPackage.Npi);
        Assert.AreEqual((byte)0, callingPartyNumberPackage.PresInd);
        Assert.AreEqual((byte)3, callingPartyNumberPackage.Screening);
        Assert.AreEqual(0, callingPartyNumberPackage.LacLength);
        Assert.AreEqual(10, callingPartyNumberPackage.NumberOfDigits);
        Assert.AreEqual("9835913848", callingPartyNumberPackage.Value);
    }
}*/