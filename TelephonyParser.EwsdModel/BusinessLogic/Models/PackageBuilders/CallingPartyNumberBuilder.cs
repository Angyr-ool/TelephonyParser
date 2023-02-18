using System.Text;
using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

public class CallingPartyNumberBuilder : IPackageBuilder
{
    public byte PackageNumber => CallingPartyNumber.Startbyte;

    private const int PackageLengthPosition = 1;
    private const int NadiPosition = 2;
    private const int NpiPresIndScreeningPosition = 3;
    private const int LacLengthNumberOfDigitsPosition = 4;
    private const int PackedDigitsPosition = 5;

    public IRecordPackage BuildPackage(byte[] recordBytes, int packageNumberPosition)
    {
        var packageLength = recordBytes[packageNumberPosition + PackageLengthPosition];

        var nadiByte = recordBytes[packageNumberPosition + NadiPosition];
        var nadi = GetNadi(nadiByte);

        var npiPresIndScreeningByte = recordBytes[packageNumberPosition + NpiPresIndScreeningPosition];
        var (npi, presInd, screening) = GetNpiPresIndScreening(npiPresIndScreeningByte);

        var lacLengthNumberOfDigitsByte = recordBytes[packageNumberPosition + LacLengthNumberOfDigitsPosition];
        var (lacLength, numberOfDigits) = GetLacLengthNumberOfDigits(lacLengthNumberOfDigitsByte);

        var packedDigitsBytesCount = packageLength - PackedDigitsPosition;
        var packedDigitsBytes = recordBytes.AsSpan(packageNumberPosition + PackedDigitsPosition, packedDigitsBytesCount);

        var packedDigits = GetPackedDigits(packedDigitsBytes.ToArray());

        return new CallingPartyNumber(packageLength, nadi, npi, presInd, screening, lacLength, numberOfDigits, packedDigits);
    }

    public static byte GetNadi(byte nadiByte)
    {
        nadiByte = (byte)(nadiByte << 1);
        nadiByte = (byte)(nadiByte >> 1);

        return nadiByte;
    }

    public static (byte Npi, byte PresInd, byte Screening) GetNpiPresIndScreening(byte npiPresIndScreeningByte)
    {
        var npiByte = (byte)(npiPresIndScreeningByte << 1);
        npiByte = (byte)(npiByte >> 5);

        var presIndByte = (byte)(npiPresIndScreeningByte << 4);
        presIndByte = (byte)(presIndByte >> 6);

        var screeningByte = (byte)(npiPresIndScreeningByte << 6);
        screeningByte = (byte)(screeningByte >> 6);

        return new(npiByte, presIndByte, screeningByte);
    }

    public static (int LacLength, int NumberOfDigits) GetLacLengthNumberOfDigits(byte lacLengthNumberOfDigitsByte)
    {
        var lacLengthByte = (byte)(lacLengthNumberOfDigitsByte >> 5);

        var numberOfDigitsByte = (byte)(lacLengthNumberOfDigitsByte << 3);
        numberOfDigitsByte = (byte)(numberOfDigitsByte >> 3);

        return new(lacLengthByte, numberOfDigitsByte);
    }

    public static string GetPackedDigits(byte[] packedDigitsBytes)
    {
        var sb = new StringBuilder();
        foreach (var packedDigitsByte in packedDigitsBytes)
        {
            var rightPartByte = (byte)(packedDigitsByte >> 4);
            sb.Append(rightPartByte);

            var leftPartByte = (byte)(packedDigitsByte & 0x0F);
            sb.Append(leftPartByte);
        }
        return sb.ToString();
    }
}
