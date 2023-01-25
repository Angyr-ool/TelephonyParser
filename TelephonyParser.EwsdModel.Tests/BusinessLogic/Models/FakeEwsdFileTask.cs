using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

internal class FakeEwsdFileTask : IEwsdFileTask
{
    public IEwsdFile? File { get; set; }
    public EwsdFileTaskStatus? Status { get; set; }
    public string? Message { get; set; }
}
