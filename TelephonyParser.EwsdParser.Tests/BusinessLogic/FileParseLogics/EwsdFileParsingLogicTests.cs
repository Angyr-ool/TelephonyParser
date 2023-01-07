using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelephonyParser.EwsdModel;
using TelephonyParser.EwsdParser.BusinessLogic;
using TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;
using TelephonyParser.EwsdParser.Infrastructure;

namespace TelephonyParser.EwsdParser.Tests.BusinessLogic.FileParseLogics;

[TestClass()]
public class EwsdFileParsingLogicTests
{
    /// <summary>
    /// Обрабатываемая задача
    /// </summary>
    private EwsdFileTask? _ewsdFileTask;

    /// <summary>
    /// Список сохраненных записей EwsdRecord
    /// </summary>
    private List<EwsdRecord>? _savedRecords;

    /// <summary>
    /// Этот метод запускается перед вызовом каждого тестового метода
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        _ewsdFileTask = new EwsdFileTask
        {
            Id = 1,
            File = new EwsdFile
            {
                Id = 1,
                Path = "path/fileName"
            },
        };

        _savedRecords = new List<EwsdRecord>();
    }

    /// <summary>
    /// Случай, когда задача (файл) успешно обрабатывается
    /// </summary>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_Success_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                // из байтов файла можно сформировать хотя бы одну запись (в виде байтов)
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                // из записей в виде байтов можно сформировать хотя бы один экземпляр EwsdRecord (запись)
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               // имитация сохранения записей на внешний источник
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(1, _savedRecords?.Count); // ожидается, что сохраненных записей будет 1 шт
        Assert.AreEqual(EwsdFileProcessStatus.Processed, _ewsdFileTask?.Status); // ожидается, что после успешной обработки статус задачи будет Processed (задача обработана)
        Assert.IsNull(_ewsdFileTask?.Message); // если задача успешно обработана, то Message будет пустой (null)
    }

    /// <summary>
    /// Случай, когда путь к обрабатываемому файлу пустой (не указан)
    /// </summary>
    /// <param name="filePath">Путь к файлу в файловой системе</param>
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void EwsdFileParsingLogic_ParseFile_EmptyFilePath_Test(string? filePath)
    {
        // указываем, что путь к файлу пустой
        _ewsdFileTask!.File!.Path = filePath;

        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе (выполнение кода до этого метода не дойдет)

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("Расположение файла не указано (task id: '1', file id: '1')", _ewsdFileTask?.Message);
    }

    /// <summary>
    /// Случай, когда путь к обрабатываемому файлу указан, но файла в файловой системе не существует
    /// </summary>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_FileIsNotExists_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => false); // файла не существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт (выполнение кода до этого метода не дойдет)
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("Файла не существует в 'path/fileName' (task id: '1', file id: '1')", _ewsdFileTask?.Message);
    }

    /// <summary>
    /// Случай, когда путь к обрабатываемому файлу указан, файл в файловой системе существует, но файл не содержит байтов (пустой)
    /// </summary>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_FileIsEmpty_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) => Array.Empty<byte>()); // файл не содержит байтов

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                // из байтов файла можно сформировать хотя бы одну запись (в виде байтов) (выполнение кода до этого метода не дойдет)
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("Файл 'path/fileName' не содержит байтов (пустой) (task id: '1', file id: '1')", _ewsdFileTask?.Message);
    }

    /// <summary>
    /// Случай, когда байты файла невозможно разбить на байты записи ewsd (см. документацию ewsd)
    /// </summary>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_NoByteRecords_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) => Array.Empty<byte[]>()); // из байтов файла невозможно сформировать хотя бы одну запись (в виде байтов)

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                // из записей в виде байтов можно сформировать хотя бы один экземпляр EwsdRecord (запись) (выполнение кода до этого метода не дойдет)
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("Файл 'path/fileName' не содержит записей (массивы байтов) (task id: '1', file id: '1')", _ewsdFileTask?.Message);
    }

    /// <summary>
    /// Случай, когда из байтов записи невозможно сформировать экземпляр EwsdRecord
    /// </summary>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_NoEwsdRecords_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                // из байтов файла можно сформировать хотя бы одну запись (в виде байтов)
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) => Array.Empty<EwsdRecord>()); // из записей в виде байтов невозможно сформировать хотя бы один экземпляр EwsdRecord (запись)

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] records) =>
           {
               // имитация сохранения записей на внешний источник (выполнение кода до этого метода не дойдет)
               _savedRecords?.AddRange(records);
           });

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("Файл 'path/fileName' не содержит записей (экземпляров EwsdRecord) (task id: '1', file id: '1')", _ewsdFileTask?.Message);
    }

    /// <summary>
    /// Случай, когда сохранения записей на внешний источник не происходит
    /// </summary>
    /// <exception cref="InvalidOperationException">Исключение, возникающее во время неудачного сохранения записей</exception>
    [TestMethod()]
    public void EwsdFileParsingLogic_ParseFile_SaveNotWorking_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileExists(It.IsAny<string?>()))
            .Returns((string? _) => true); // файл существует в файловой системе

        mock
            .Mock<IFileSystem>()
            .Setup(x => x.FileReadAllBytes(It.IsAny<string?>()))
            .Returns((string? _) =>
            {
                // файл содержит хотя бы 1 байт
                return new byte[] { 1 };
            });

        mock
            .Mock<IEwsdFileBytesSplitLogic>()
            .Setup(x => x.SplitBytes(It.IsAny<byte[]>()))
            .Returns((byte[] _) =>
            {
                // из байтов файла можно сформировать хотя бы одну запись (в виде байтов)
                return new[] { new byte[] { 1 } };
            });

        mock
            .Mock<IEwsdRecordsBuildService>()
            .Setup(x => x.BuildRecords(It.IsAny<byte[][]>()))
            .Returns((byte[][] _) =>
            {
                // из записей в виде байтов можно сформировать хотя бы один экземпляр EwsdRecord (запись)
                return new[] { new EwsdRecord() };
            });

        mock
           .Mock<IEwsdRecordService>()
           .Setup(x => x.Save(It.IsAny<EwsdRecord[]>()))
           .Callback((EwsdRecord[] _) => throw new InvalidOperationException("save records is not working"));// во время сохранения записей возникает исключение

        var sut = mock.Create<EwsdFileParsingLogic>();

        // Act
        sut.ParseFile(_ewsdFileTask!);
        //var ex = Assert.ThrowsException<InvalidOperationException>(() => sut.ParseFile(_ewsdFileTask!));

        // Assert
        Assert.AreEqual(0, _savedRecords?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTask?.Status);
        Assert.AreEqual("save records is not working", _ewsdFileTask?.Message);
    }
}