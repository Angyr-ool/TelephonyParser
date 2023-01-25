using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.Models;

internal class EwsdExternalResourceService : IEwsdExternalResourceService
{
    public IEwsdFileTask? GetNew()
    {
        return new EwsdFileTask
        {
            File = new EwsdFile
            {
                Directory = "Dir",
                Name = "File",
                Path = Path.Combine("Dir", "File")
            },
            Message = null,
            Status = EwsdFileTaskStatus.New
        };
    }

    public async Task<IEwsdFileTask?> GetNewAsync()
    {
        await Task.Delay(500);

        return new EwsdFileTask
        {
            File = new EwsdFile
            {
                Directory = "Dir",
                Name = "File",
                Path = Path.Combine("Dir", "File")
            },
            Message = null,
            Status = EwsdFileTaskStatus.New
        };
    }

    public void Save(IEwsdFileTask ewsdFileTask)
    {
        
    }

    public void Save(IEwsdRecord[] records)
    {
        
    }

    public async Task SaveAsync(IEwsdFileTask ewsdFileTask, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
    }
}

internal class EwsdFileTask : IEwsdFileTask
{
    public IEwsdFile? File { get; set; }
    public EwsdFileTaskStatus? Status { get; set; }
    public string? Message { get; set; }
}

internal class EwsdFile : IEwsdFile
{
    public string? Name { get; set; }
    public string? Directory { get; set; }
    public string? Path { get; set; }
}
