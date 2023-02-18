using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;

public class BuildRecordLogic : IBuildRecordLogic
{
    public IRecord? Build(IRecordPackage[] packages)
    {
        throw new NotImplementedException();
    }
}
