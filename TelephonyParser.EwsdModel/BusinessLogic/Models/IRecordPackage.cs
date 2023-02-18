namespace TelephonyParser.EwsdModel.BusinessLogic.Models;

public interface IRecordPackage //: IDisposable, ICloneable
{
    /// <summary>
    /// Номер пакета
    /// </summary>
    byte Number { get; }

    /// <summary>
    /// Длина пакета (количество занимаемых пакетом байтов в записи)
    /// </summary>
    int Length { get; }
}
