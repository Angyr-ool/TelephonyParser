using TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

public class DateTimeDurationBuilder : IPackageBuilder
{
    public byte PackageNumber => DateTimeDuration.Startbyte;

    private const int DatetimeBytePosition = 1;
    private const int FlagsBytePosition = 7;
    private const int DurationBytePosition = 8;
    private const int DurationLength = 3;

    public IRecordPackage BuildPackage(byte[] recordBytes, int packageNumberPosition)
    {
        if (packageNumberPosition + DateTimeDuration.PackageLength > recordBytes.Length)
        {
            throw new ArgumentException($"Некорректное значение {packageNumberPosition}");
        }

        var flagsByte = recordBytes[packageNumberPosition + FlagsBytePosition];
        var flags = new DateTimeDurationFlags(flagsByte);

        var dateTimeBytes = recordBytes.AsSpan().Slice(packageNumberPosition + DatetimeBytePosition, 6);
        var dateTime = GetDateTime(dateTimeBytes.ToArray(), flags.F0, flags.TenthsOfSeconds);

        var durationBytes = recordBytes.AsSpan().Slice(packageNumberPosition + DurationBytePosition, DurationLength);
        var duration = GetDuration(durationBytes.ToArray());

        return new DateTimeDuration(dateTime, flags, duration);
    }

    /// <summary>
    /// Вернуть дату и время вызова | Field 2
    /// </summary>
    /// <param name="dateTimeBytes">Байты поля</param>
    /// <param name="f0">true  - год начиная с 2000 <br />
    /// false - год между 1900 и 1999
    /// </param>
    /// <param name="tenthsOfSeconds">десятые доли секунды</param>
    /// <returns></returns>
    public static DateTime GetDateTime(byte[] dateTimeBytes, bool f0, int tenthsOfSeconds)
    {
        int year;

        if (f0) 
        {
            year = dateTimeBytes[0] + 2000;
        }
        else
        {
            year = dateTimeBytes[0] + 1900;
        }
        
        var month = dateTimeBytes[1];
        var day = dateTimeBytes[2];
        var hour = dateTimeBytes[3];
        var minute = dateTimeBytes[4];
        var second = dateTimeBytes[5];

        return new (year, month, day, hour, minute, second, tenthsOfSeconds * 100);
    }

    /// <summary>
    /// Вернуть продолжительность вызова | Field 4
    /// </summary>
    /// <param name="durationBytes">Байты продолжительности вызова</param>
    /// <returns>Duration</returns>
    public static int GetDuration(byte[] durationBytes)
    {
        durationBytes = durationBytes.Reverse().ToArray();

        var durationString = BitConverter.ToString(durationBytes);

        string duration = durationString.Replace("-", "");

        return Convert.ToInt32(duration, 16);
    }
}
