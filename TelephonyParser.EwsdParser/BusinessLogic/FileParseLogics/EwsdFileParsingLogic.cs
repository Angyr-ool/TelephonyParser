using Microsoft.Extensions.Logging;
using TelephonyParser.EwsdModel;
using TelephonyParser.EwsdParser.Infrastructure;

namespace TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;

public class EwsdFileParsingLogic : IEwsdFileParsingLogic
{
    private readonly ILogger<EwsdFileParsingLogic> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly IEwsdFileBytesSplitLogic _fileBytesSplitLogic;
    private readonly IEwsdRecordsBuildService _recordsBuildService;
    private readonly IEwsdRecordService _recordService;

    public EwsdFileParsingLogic(ILogger<EwsdFileParsingLogic> logger, IFileSystem fileSystem, IEwsdFileBytesSplitLogic fileBytesSplitLogic, IEwsdRecordsBuildService recordsBuildService, IEwsdRecordService recordService)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _fileBytesSplitLogic = fileBytesSplitLogic;
        _recordsBuildService = recordsBuildService;
        _recordService = recordService;
    }

    public void ParseFile(EwsdFileTask ewsdFileTask)
    {
        try
        {
            _logger.LogInformation($"Parsing ewsd file: {ewsdFileTask.File?.Path} ...");
            ewsdFileTask.Status = EwsdFileProcessStatus.OnProcess;

            if (string.IsNullOrEmpty(ewsdFileTask.File?.Path))
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Error;
                ewsdFileTask.Message = $"Расположение файла не указано (task id: '{ewsdFileTask.Id}', file id: '{ewsdFileTask.File?.Id}')";
                return;
            }

            if (!_fileSystem.FileExists(ewsdFileTask.File.Path))
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Error;
                ewsdFileTask.Message = $"Файла не существует в '{ewsdFileTask.File.Path}' (task id: '{ewsdFileTask.Id}', file id: '{ewsdFileTask.File.Id}')";
                return;
            }

            var fileBytes = _fileSystem.FileReadAllBytes(ewsdFileTask.File.Path);

            if (fileBytes.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит байтов (пустой) (task id: '{ewsdFileTask.Id}', file id: '{ewsdFileTask.File.Id}')";
                return;
            }

            var fileBytesArray = _fileBytesSplitLogic.SplitBytes(fileBytes);

            if (fileBytesArray.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит записей (массивы байтов) (task id: '{ewsdFileTask.Id}', file id: '{ewsdFileTask.File.Id}')";
                return;
            }

            var records = _recordsBuildService.BuildRecords(fileBytesArray);

            if (records.Length == 0)
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Error;
                ewsdFileTask.Message = $"Файл '{ewsdFileTask.File.Path}' не содержит записей (экземпляров EwsdRecord) (task id: '{ewsdFileTask.Id}', file id: '{ewsdFileTask.File.Id}')";
                return;
            }

            ewsdFileTask.Status = EwsdFileProcessStatus.Processed;
            _recordService.Save(records);
        }
        catch (Exception e)
        {
            ewsdFileTask.Status = EwsdFileProcessStatus.Error;
            ewsdFileTask.Message = e.Message;
        }
    }

    public async Task ParseFileAsync(EwsdFileTask ewsdFileTask, CancellationToken cancellationToken)
    {
        await Task.Run(() => ParseFile(ewsdFileTask), cancellationToken);
        await Task.Delay(5000, cancellationToken);
    }
}