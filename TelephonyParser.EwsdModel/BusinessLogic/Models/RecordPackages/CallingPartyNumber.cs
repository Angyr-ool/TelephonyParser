namespace TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

/// <summary>
/// Package 142 (H’8E)
/// </summary>
public class CallingPartyNumber : IRecordPackage
{
    public const int Startbyte = 142;
    public byte Number => Startbyte;

    public CallingPartyNumber(int packageLength, byte nadi, byte npi, byte presInd, byte screening, int lacLength, int numberOfDigits, string packedDigits)
    {
        Length = packageLength;
        Nadi = nadi;
        Npi = npi;
        PresInd = presInd;
        Screening = screening;
        LacLength = lacLength;
        NumberOfDigits = numberOfDigits;
        PackedDigits = packedDigits;
    }

    public int Length { get; private set; }
    public byte Nadi { get; private set; }
    public byte Npi { get; private set; }
    public byte PresInd { get; private set; }
    public byte Screening { get; private set; }
    public int LacLength { get; private set; }
    public int NumberOfDigits { get; private set; }
    public string PackedDigits { get; private set; }
}