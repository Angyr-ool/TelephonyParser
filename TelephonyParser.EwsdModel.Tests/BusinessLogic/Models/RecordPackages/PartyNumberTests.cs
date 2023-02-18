/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models.RecordPackages;

[TestClass()]
public class PartyNumberTests
{
    [DataTestMethod()]
    [DataRow(0,   (byte)0x00)]
    [DataRow(1,   (byte)0x01)]
    [DataRow(10,  (byte)0x0A)]
    [DataRow(100, (byte)0x64)]
    [DataRow(142, (byte)0x8E)]
    public void PartyNumber_GetPackageLength_Test(int expected, byte input)
    {
        Assert.AreEqual(expected, PartyNumber.GetPackageLength(input));
    }

    [DataTestMethod()]
    [DataRow((byte)0x00, (byte)0x00)]
    [DataRow((byte)0x01, (byte)0x01)]
    [DataRow((byte)0x03, (byte)0x03)]
    [DataRow((byte)0x7F, (byte)0xFF)]
    public void PartyNumber_GetNadi_Test(byte expected, byte input)
    {
        Assert.AreEqual(expected, PartyNumber.GetNadi(input));
    }

    [DataTestMethod()]
    [DataRow((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00)]
    [DataRow((byte)0x00, (byte)0x00, (byte)0x01, (byte)0x01)]
    [DataRow((byte)0x00, (byte)0x01, (byte)0x01, (byte)0x05)]
    [DataRow((byte)0x01, (byte)0x01, (byte)0x01, (byte)0x15)]
    [DataRow((byte)0x07, (byte)0x03, (byte)0x03, (byte)0x7F)]
    [DataRow((byte)0x07, (byte)0x03, (byte)0x03, (byte)0xFF)]
    public void PartyNumber_GetNpiPresIndScreening_Test(byte expectedNpi, byte expectedPresInd, byte expectedScreening, byte input)
    {
        var (Npi, PresInd, Screening) = PartyNumber.GetNpiPresIndScreening(input);
        Assert.AreEqual(expectedNpi, Npi);
        Assert.AreEqual(expectedPresInd, PresInd);
        Assert.AreEqual(expectedScreening, Screening);
    }

    [DataTestMethod()]
    [DataRow(0, 0, (byte)0x00)]
    [DataRow(1, 1, (byte)0x21)]
    [DataRow(0, 10, (byte)0x0A)]
    [DataRow(7, 31, (byte)0xFF)]
    public void PartyNumber_GetLacLengthNumberOfDigits_Test(int expectedLacLength, int expectedNumberOfDigits, byte input)
    {
        var (LacLength, NumberOfDigits) = PartyNumber.GetLacLengthNumberOfDigits(input);
        Assert.AreEqual(expectedLacLength, LacLength);
        Assert.AreEqual(expectedNumberOfDigits, NumberOfDigits);
    }

    [TestMethod()]
    public void PartyNumber_GetPackedDigits_Test()
    {
        var packedDigits1 = PartyNumber.GetPackedDigits(new byte[] { 0x94, 0x51, 0x04, 0x90, 0x62 });
        Assert.AreEqual("9451049062", packedDigits1);

        var packedDigits2 = PartyNumber.GetPackedDigits(new byte[] { 0x94, 0x51, 0x04, 0x90, 0x00 });
        Assert.AreEqual("9451049000", packedDigits2);
    }
}*/