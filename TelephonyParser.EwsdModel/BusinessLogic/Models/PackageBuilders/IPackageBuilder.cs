using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephonyParser.EwsdModel.BusinessLogic.Models.PackageBuilders;

public interface IPackageBuilder
{
    /// <summary>
    /// Номер пакета
    /// </summary>
    byte PackageNumber { get; }

    IRecordPackage BuildPackage(byte[] recordBytes, int packageNumberPosition);
}
