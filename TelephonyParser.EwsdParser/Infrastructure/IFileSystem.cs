namespace TelephonyParser.EwsdParser.Infrastructure;

public interface IFileSystem
{
    bool DirectoryExists(string? path);
    bool FileExists(string? path);
    byte[] FileReadAllBytes(string? path);
}
