using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;
using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.BusinessLogic.SplitFileBytesLogics;
using TelephonyParser.EwsdModel.FileSystemServices;

namespace TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;

public class ParseEwsdFileLogic : IParseEwsdFileLogic
{
    private readonly IFileSystem _fileSystem;
    private readonly ISplitEwsdFileBytesLogic _splitFileBytesLogic;
    private readonly IBuildEwsdRecordsLogic _buildRecordsLogic;
    private readonly IEwsdExternalResourceService _ewsdExternalResourceService;

    public ParseEwsdFileLogic(IFileSystem fileSystem, ISplitEwsdFileBytesLogic splitFileBytesLogic, 
        IBuildEwsdRecordsLogic buildRecordsLogic, IEwsdExternalResourceService ewsdExternalResourceService)
    {
        _fileSystem = fileSystem;
        _splitFileBytesLogic = splitFileBytesLogic;
        _buildRecordsLogic = buildRecordsLogic;
        _ewsdExternalResourceService = ewsdExternalResourceService;
    }

    public void ParseFile(IEwsdFileTask ewsdFileTask)
    {
        try
        {
            if (string.IsNullOrEmpty(ewsdFileTask.File?.Path))
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Error;
                ewsdFileTask.Message = $"Расположение файла не указано";
                return;
            }

            if (!_fileSystem.IsFileExists(ewsdFileTask.File.Path))
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Error;
                ewsdFileTask.Message = $"Файла не существует в '{ewsdFileTask.File.Path}'";
                return;
            }

            var fileBytes = _fileSystem.ReadAllFileBytes(ewsdFileTask.File.Path);

            if (fileBytes.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит байтов (пустой)";
                return;
            }

            var fileBytesArray = _splitFileBytesLogic.SplitBytes(fileBytes);

            if (fileBytesArray.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит записей (массивы байтов)";
                return;
            }

            var records = _buildRecordsLogic.BuildRecords(fileBytesArray);

            if (records.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит записей (экземпляров EwsdRecord)";
                return;
            }

            ewsdFileTask.Status = EwsdFileTaskStatus.Processed;
            _ewsdExternalResourceService.Save(records);
        }
        catch (Exception e)
        {
            ewsdFileTask.Status = EwsdFileTaskStatus.Error;
            ewsdFileTask.Message = e.Message;
        }
    }

    public Task ParseFileAsync(IEwsdFileTask ewsdFileTask, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
