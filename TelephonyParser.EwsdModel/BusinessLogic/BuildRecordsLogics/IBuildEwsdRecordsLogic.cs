using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;

public interface IBuildEwsdRecordsLogic
{
    IRecord[] BuildRecords(byte[][] fileBytes);
}