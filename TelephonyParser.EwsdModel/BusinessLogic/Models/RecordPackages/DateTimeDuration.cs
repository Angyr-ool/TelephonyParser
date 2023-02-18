namespace TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

public struct DateTimeDuration : IRecordPackage
{
    /// <summary>
    /// Стартовый байт пакета (Package identifier)
    /// </summary>
    public const byte Startbyte = 0x64;

    /// <summary>
    /// Длина пакета (сколько байтов занимает пакет)
    /// </summary>
    public const int PackageLength = 11;

    public DateTimeDuration(DateTime dateTime, DateTimeDurationFlags flags, int duration)
    {
        DateTime = dateTime;
        Flags = flags;
        Duration = duration;
    }

    public byte Number { get => Startbyte; }

    public int Length => PackageLength;

    public DateTime DateTime { get; private set; }
    public DateTimeDurationFlags Flags { get; private set; }
    public int Duration { get; private set; }
}

public struct DateTimeDurationFlags
{
    /// <summary>
    /// false - год между 1900 и 1999 <br />
    /// true  - год начиная с 2000
    /// </summary>
    public bool F0 { get; private set; }

    /// <summary>
    /// десятые доли секунды
    /// </summary>
    public int TenthsOfSeconds { get; private set; }

    /// <summary>
    /// false - дата и время начала вызова <br />
    /// true  - дата и время конца вызова
    /// </summary>
    public bool F6 { get; private set; }

    /// <summary>
    /// false - time secure <br />
    /// true  - time insecure
    /// </summary>
    public bool F7 { get; private set; }

    public DateTimeDurationFlags(byte flagsByte)
    {
        (bool F0, int TenthsOfSeconds, bool F6, bool F7) res = GetFlags(flagsByte);

        F0 = res.F0;
        TenthsOfSeconds = res.TenthsOfSeconds;
        F6 = res.F6;
        F7 = res.F7;
    }

    /// <summary>
    /// Флаги пакета F0, TenthsOfSeconds, F6, F7 | Field 3
    /// </summary>
    /// <param name="flagsByte">Байт флагов</param>
    /// <returns>
    /// F0 = false - год между 1900 и 1999 <br />
    /// F0 = true  - год начиная с 2000 <br />
    /// TenthsOfSeconds - десятые доли секунды  <br />
    /// F6 = false - дата и время начала вызова <br />
    /// F6 = true  - дата и время конца вызова <br />
    /// F7 = false - time secure <br />
    /// F7 = true  - time insecure <br />
    /// </returns>
    public static (bool F0, int TenthsOfSeconds, bool F6, bool F7) GetFlags(byte flagsByte)
    {
        bool f0 = (flagsByte & (1 << 0)) != 0;
        byte tenthsOfSeconds = (byte)(flagsByte >> 1);
        tenthsOfSeconds = (byte)(tenthsOfSeconds << 4);
        tenthsOfSeconds = (byte)(tenthsOfSeconds >> 4);
        bool f6 = (flagsByte & (1 << 6)) != 0;
        bool f7 = (flagsByte & (1 << 7)) != 0;

        return new(f0, tenthsOfSeconds, f6, f7);
    }
}