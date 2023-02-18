using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

public class FixedPartBuilder : IPackageBuilder
{
    private const int RecordLengthBytePosition = 1;
    private const int FlagsBytePosition = 3;
    private const int RecordSequenceAndChargeStatusBytePosition = 6;
    private const int RecordOwnerTypeBytePosition = 7;

    public byte PackageNumber { get => FixedPart.Startbyte; }

    public IRecordPackage BuildPackage(byte[] recordBytes, int packageNumberPosition)
    {
        if (packageNumberPosition + FixedPart.PackageLength > recordBytes.Length)
        {
            throw new ArgumentException($"Некорректное значение {packageNumberPosition}");
        }

        var recordLengthBytes = recordBytes.AsSpan().Slice(packageNumberPosition + RecordLengthBytePosition, 2);

        var recordLength = GetRecordLength(recordLengthBytes.ToArray());

        var f11f18Byte = recordBytes[packageNumberPosition + FlagsBytePosition];
        var f21f27Byte = recordBytes[packageNumberPosition + FlagsBytePosition + 1];
        var f33f38Byte = recordBytes[packageNumberPosition + FlagsBytePosition + 2];
        var flags = new FixedPartFlags(f11f18Byte, f21f27Byte, f33f38Byte);

        var recordSequenceAndChargeStatusByte = recordBytes[packageNumberPosition + RecordSequenceAndChargeStatusBytePosition];
        var (recordSequence, chargeStatus) = GetRecordSequenceAndChargeStatus(recordSequenceAndChargeStatusByte);

        var recordOwnerType = recordBytes[packageNumberPosition + RecordOwnerTypeBytePosition];

        return new FixedPart(recordLength, flags, recordSequence, chargeStatus, recordOwnerType);
    }

    /// <summary>
    /// Вернуть длину записи (Record Length) | Field 2
    /// </summary>
    /// <param name="fixedPartLengthBytes"></param>
    /// <returns></returns>
    public static int GetRecordLength(byte[] fixedPartLengthBytes)
    {
        if (fixedPartLengthBytes.Length == 2)
        {
            return fixedPartLengthBytes[0] + fixedPartLengthBytes[1];
        }

        return 0;
    }

    /// <summary>
    /// Вернуть RecordSequence и ChargeStatus | Field 4
    /// </summary>
    /// <param name="recordSequenceAndChargeStatusByte">Байт RecordSequence и ChargeStatus</param>
    /// <returns></returns>
    public static (byte RecordSequence, byte ChargeStatus) GetRecordSequenceAndChargeStatus(byte recordSequenceAndChargeStatusByte)
    {
        var recordSequence = (byte)(recordSequenceAndChargeStatusByte >> 4);

        var rightPart = (byte)(recordSequenceAndChargeStatusByte << 4);

        var chargeStatus = (byte)(rightPart >> 4);

        return new(recordSequence, chargeStatus);
    }
}
