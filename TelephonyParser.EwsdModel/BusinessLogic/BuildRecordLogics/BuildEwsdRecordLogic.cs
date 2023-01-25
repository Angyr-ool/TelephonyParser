using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;

public class BuildEwsdRecordLogic : IBuildEwsdRecordLogic
{
    public IEwsdRecord? Build(IEwsdPackage[] packages)
    {
        throw new NotImplementedException();
    }
}
