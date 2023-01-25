namespace TelephonyParser.EwsdModel.BusinessLogic.Models;

public interface IEwsdPackage : IDisposable, ICloneable
{
    /// <summary>
    /// Начальный байт
    /// </summary>
    byte StartByte { get; }

    /// <summary>
    /// Вернуть длину пакета (количество занимаемых байтов пакетом в записи)
    /// </summary>
    /// <param name="record">запись в виде байтов</param>
    /// <param name="startBytePositionInRecord">позиция начального байта в записи</param>
    /// <returns>Длина пакета</returns>
    byte GetLength(in byte[] record, int startBytePositionInRecord);
}
