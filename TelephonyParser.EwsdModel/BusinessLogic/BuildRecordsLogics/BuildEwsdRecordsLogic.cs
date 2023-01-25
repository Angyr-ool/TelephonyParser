using TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;
using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;
using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;

public class BuildEwsdRecordsLogic : IBuildEwsdRecordsLogic
{
    private readonly IBuildEwsdPackagesLogic _packagesBuilder;
    private readonly IBuildEwsdRecordLogic _recordBuildService;

    public BuildEwsdRecordsLogic(IBuildEwsdPackagesLogic packagesBuilder, IBuildEwsdRecordLogic recordBuildService)
    {
        _packagesBuilder = packagesBuilder;
        _recordBuildService = recordBuildService;
    }

    public IEwsdRecord[] BuildRecords(byte[][] fileBytes)
    {
        var records = new List<IEwsdRecord>();

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