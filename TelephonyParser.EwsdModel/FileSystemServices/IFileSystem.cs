using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephonyParser.EwsdModel.FileSystemServices;

public interface IFileSystem
{
    bool IsDirectoryExists(string? directory);
    bool IsFileExists(string? path);
    byte[] ReadAllFileBytes(string? path);
}
