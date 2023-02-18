using System;
using System.Collections;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.RecordPackages;

/// <summary>
/// Пакет 100 (H'84)
/// </summary>
public class FixedPart : IRecordPackage
{
    /// <summary>
    /// Идентификатор записи (Record Identifier)
    /// </summary>
    public const byte Startbyte = 0x84;

    /// <summary>
    /// Длина пакета
    /// </summary>
    public const int PackageLength = 8;

    public byte Number { get => Startbyte; }

    public int Length { get => PackageLength; }

    public FixedPart(int recordLength, FixedPartFlags flags, byte recordSequence, byte chargeStatus, byte recordOwnerType)
    {
        RecordLength = recordLength;
        Flags = flags;
        RecordSequence = recordSequence;
        ChargeStatus = chargeStatus;
        RecordOwnerType = recordOwnerType;
    }

    public int RecordLength { get; private set; }
    public FixedPartFlags Flags { get; private set; }
    public byte RecordSequence { get; private set; }
    public byte ChargeStatus { get; private set; }
    public byte RecordOwnerType { get; private set; }
}

public class FixedPartFlags
{
    public FixedPartFlags(byte f11f18Byte, byte f21f27Byte, byte f33f38Byte)
    {
        var (f11, f13, f14, f15, f16, f17, f18) = GetFlagsFromF11ToF18(f11f18Byte);
        F11 = f11;
        F13 = f13;
        F14 = f14;
        F15 = f15;
        F15 = f15;
        F16 = f16;
        F17 = f17;
        F18 = f18;

        var (f21, f22, f23, f24, f25, f27) = GetFlagsFromF21ToF27(f21f27Byte);
        F21 = f21;
        F22 = f22;
        F23 = f23;
        F24 = f24;
        F25 = f25;
        F27 = f27;

        var (f33, f34, f35, f37, f38) = GetFlagsFromF33ToF38(f33f38Byte);
        F33 = f33;
        F34 = f34;
        F35 = f35;
        F37 = f37;
        F38 = f38;
    }

    public bool F11 { get; private set; }
    public bool F13 { get; private set; }
    public bool F14 { get; private set; }
    public bool F15 { get; private set; }
    public bool F16 { get; private set; }
    public bool F17 { get; private set; }
    public bool F18 { get; private set; }

    public bool F21 { get; private set; }
    public bool F22 { get; private set; }
    public bool F23 { get; private set; }
    public bool F24 { get; private set; }
    public bool F25 { get; private set; }
    public bool F27 { get; private set; }

    public bool F33 { get; private set; }
    public bool F34 { get; private set; }
    public bool F35 { get; private set; }
    public bool F37 { get; private set; }
    public bool F38 { get; private set; }

    /// <summary>
    /// Флаги F11-F18
    /// </summary>
    /// <param name="flagsByte">Байт флагов F11-F18</param>
    /// <returns></returns>
    public static (bool F11, bool F13, bool F14, bool F15, bool F16, bool F17, bool F18) GetFlagsFromF11ToF18(byte flagsByte)
    {
        bool f11 = (flagsByte & (1 << 0)) != 0;
        bool f13 = (flagsByte & (1 << 2)) != 0;
        bool f14 = (flagsByte & (1 << 3)) != 0;
        bool f15 = (flagsByte & (1 << 4)) != 0;
        bool f16 = (flagsByte & (1 << 5)) != 0;
        bool f17 = (flagsByte & (1 << 6)) != 0;
        bool f18 = (flagsByte & (1 << 7)) != 0;

        return new(f11, f13, f14, f15, f16, f17, f18);
    }

    /// <summary>
    /// Флаги F21-F27
    /// </summary>
    /// <param name="flagsByte">Байт флагов F21-F27</param>
    /// <returns></returns>
    public static (bool F21, bool F22, bool F23, bool F24, bool F25, bool F27) GetFlagsFromF21ToF27(byte flagsByte)
    {
        bool f21 = (flagsByte & (1 << 0)) != 0;
        bool f22 = (flagsByte & (1 << 1)) != 0;
        bool f23 = (flagsByte & (1 << 2)) != 0;
        bool f24 = (flagsByte & (1 << 3)) != 0;
        bool f25 = (flagsByte & (1 << 4)) != 0;
        bool f27 = (flagsByte & (1 << 6)) != 0;

        return new(f21, f22, f23, f24, f25, f27);
    }

    /// <summary>
    /// Флаги F33-F38
    /// </summary>
    /// <param name="flagsByte">Байт флагов F33-F38</param>
    /// <returns></returns>
    public static (bool F33, bool F34, bool F35, bool F37, bool F38) GetFlagsFromF33ToF38(byte flagsByte)
    {
        bool f33 = (flagsByte & (1 << 2)) != 0;
        bool f34 = (flagsByte & (1 << 3)) != 0;
        bool f35 = (flagsByte & (1 << 4)) != 0;
        bool f37 = (flagsByte & (1 << 6)) != 0;
        bool f38 = (flagsByte & (1 << 7)) != 0;

        return new(f33, f34, f35, f37, f38);
    }
}