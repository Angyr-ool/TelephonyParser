using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

public class FakeEwsdFile : IEwsdFile
{
    public string? Name { get; set; }
    public string? Directory { get; set; }
    public string? Path { get; set; }
}
