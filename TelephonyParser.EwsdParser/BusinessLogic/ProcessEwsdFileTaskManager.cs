using TelephonyParser.EwsdModel;

namespace TelephonyParser.EwsdParser.BusinessLogic;

public class ProcessEwsdFileTaskManager : IProcessEwsdFileTaskManager
{
    public EwsdFileTask? GetNew()
    {
        return new EwsdFileTask
        {
            File = new EwsdFile
            {
                Name = "validFileName",
                Directory = "path/",
                Path = "path/validFileName"
            },
            Status = EwsdFileProcessStatus.New,
            Message = null
        };
    }

    public void Save(EwsdFileTask fileTask)
    {

    }
}