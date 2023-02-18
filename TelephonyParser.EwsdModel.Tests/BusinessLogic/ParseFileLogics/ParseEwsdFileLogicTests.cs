using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;
using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;
using TelephonyParser.EwsdModel.BusinessLogic.SplitFileBytesLogics;
using TelephonyParser.EwsdModel.FileSystemServices;
using TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.ParseFileLogics;

[TestClass()]
public class ParseEwsdFileLogicTests
{
    /// <summary>
    /// Обрабатываемая задача
    /// </summary>
    private FakeEwsdFileTask? _ewsdFileTask;

    /// <summary>
    /// Список сохраненных записей EwsdRecord
    /// </summary>
    private List<IRecord>? _savedRecords;

    /// <summary>
    /// Этот метод запускается перед вызовом каждого тестового метода
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        _ewsdFileTask = new FakeEwsdFileTask
        {
            File = new FakeEwsdFile
            {
                Path = "path/fileName"
            },
            Status = EwsdFileTaskStatus.OnProcess
        };

        _savedRecords = new List<IRecord>();
    }

    [TestMethod()]
    public void ParseEwsdFileLogic_ParseFile_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.IsFileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.ReadAllFileBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт
                return new byte[] { 1 };
            });

        mock
            .Mock<ISplitEwsdFileBytesLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                // из байтов файла можно сформировать хотя бы одну запись (в виде байтов)
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IBuildEwsdRecordsLogic>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                // из записей в виде байтов можно сформировать хотя бы один экземпляр EwsdRecord (запись)
                return new[] { new FakeEwsdRecord() };
            });

        mock
           .Mock<IEwsdExternalResourceService>()
           .Setup(x => x.Save(It.IsAny<IRecord[]>()))
           .Callback((IRecord[] records) =>
           {
               // имитация сохранения записей на внешний источник
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<ParseEwsdFileLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(1, _savedRecords?.Count); // ожидается, что сохраненных записей будет 1 шт
        Assert.AreEqual(EwsdFileTaskStatus.Processed, _ewsdFileTask?.Status); // ожидается, что после успешной обработки статус задачи будет Processed (задача обработана)
        Assert.IsNull(_ewsdFileTask?.Message); // если задача успешно обработана, то Message будет пустой (null)
    }
}