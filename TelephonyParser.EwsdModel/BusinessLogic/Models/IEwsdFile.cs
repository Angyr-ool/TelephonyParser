namespace TelephonyParser.EwsdModel.BusinessLogic.Models;

public interface IEwsdFile
{
    string? Name { get; set; }
    string? Directory { get; set; }
    string? Path { get; set; }
}
