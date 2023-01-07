using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic;

public interface IEwsdRecordsBuildService
{
    EwsdRecord[] BuildRecords(byte[][] fileBytes);
}
