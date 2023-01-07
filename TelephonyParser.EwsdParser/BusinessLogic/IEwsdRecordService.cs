using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic;

public interface IEwsdRecordService
{
    void Save(EwsdRecord[] records);
}
