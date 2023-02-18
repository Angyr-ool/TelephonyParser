namespace TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

/// <summary>
/// Package 168 (H’A8)
/// </summary>
public class CalledPartyNumber : IRecordPackage
{
    public const byte Startbyte = 0xA8;

    public CalledPartyNumber(byte packageLength, byte nadi, byte npi, int lacLength, int numberOfDigits, string packedDigits)
    {
        Length = packageLength;
        Nadi = nadi;
        Npi = npi;
        LacLength = lacLength;
        NumberOfDigits = numberOfDigits;
        PackedDigits = packedDigits;
    }

    public byte Number => Startbyte;
    public int Length { get; private set; }

    public byte Nadi { get; private set; }
    public byte Npi { get; private set; }
    public int LacLength { get; private set; }
    public int NumberOfDigits { get; private set; }
    public string PackedDigits { get; private set; }
}
