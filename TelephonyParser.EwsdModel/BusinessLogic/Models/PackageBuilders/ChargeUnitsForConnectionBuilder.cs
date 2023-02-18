using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

public class ChargeUnitsForConnectionBuilder : IPackageBuilder
{
    public byte PackageNumber => ChargeUnitsForConnection.Startbyte;

    private const int CallChargeUnitsPosition = 1;
    private const int CallChargeUnitsLength = 3;

    public IRecordPackage BuildPackage(byte[] recordBytes, int packageNumberPosition)
    {
        var callChargeUnitsBytes = recordBytes.AsSpan(packageNumberPosition + CallChargeUnitsPosition, CallChargeUnitsLength);
        var callChargeUnits = GetCallChargeUnits(callChargeUnitsBytes.ToArray());
        return new ChargeUnitsForConnection(callChargeUnits);
    }

    public static int GetCallChargeUnits(byte[] callChargeUnitsBytes)
    {
        callChargeUnitsBytes = callChargeUnitsBytes.Reverse().ToArray();

        var durationString = BitConverter.ToString(callChargeUnitsBytes);

        string duration = durationString.Replace("-", "");

        return Convert.ToInt32(duration, 16);
    }
}
