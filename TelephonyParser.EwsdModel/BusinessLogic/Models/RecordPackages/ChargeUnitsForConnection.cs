namespace TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

/// <summary>
/// Package 103 (H’67)
/// </summary>
public class ChargeUnitsForConnection : IRecordPackage
{
    public const int Startbyte = 103;
    public const int PackageLength = 4;

    public byte Number => Startbyte;
    public int Length => PackageLength;

    public int CallChargeUnits { get; private set; }

    public ChargeUnitsForConnection(int callChargeUnits)
    {
        CallChargeUnits = callChargeUnits;
    }
}
