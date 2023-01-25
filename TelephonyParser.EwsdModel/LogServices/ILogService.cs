using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;

namespace TelephonyParser.EwsdModel.LogServices;

public interface ILogService<T>
{
    void LogInformation(string message);
    void LogCritical(string message);
    void LogError(string message);
}

/*public class LogService<T> : ILogService<T>
{
    public void LogCritical(T type, string message)
    {
        switch (type)
        {
            case IProcessEwsdFilesLogic:

            default:
                break;
        }

        var r = typeof(IProcessEwsdFilesLogic);
    }
    public void LogError(T type, string message);
    public void LogInformation(T type, string message);
}*/