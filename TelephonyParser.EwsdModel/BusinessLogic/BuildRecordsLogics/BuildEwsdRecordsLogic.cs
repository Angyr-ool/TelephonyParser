using TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;
using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;
using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;

public class BuildEwsdRecordsLogic : IBuildEwsdRecordsLogic
{
    private readonly IBuildPackagesLogic _packagesBuilder;
    private readonly IBuildRecordLogic _recordBuildService;

    public BuildEwsdRecordsLogic(IBuildPackagesLogic packagesBuilder, IBuildRecordLogic recordBuildService)
    {
        _packagesBuilder = packagesBuilder;
        _recordBuildService = recordBuildService;
    }

    public IRecord[] BuildRecords(byte[][] fileBytes)
    {
        var records = new List<IRecord>();

        foreach (var recordBytes in fileBytes)
        {
            var packages = _packagesBuilder.Build(recordBytes);

            if (packages.Length == 0)
            {
                continue;
            }

            var record = _recordBuildService.Build(packages);

            if (record is null)
            {
                continue;
            }

            records.Add(record);
        }

        return records.ToArray();
    }
}