using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelephonyParser.EwsdModel;
using TelephonyParser.EwsdParser.BusinessLogic;
using TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;
using TelephonyParser.EwsdParser.BusinessLogic.FilesProcessLogics;

namespace TelephonyParser.EwsdParser.Tests.BusinessLogic.FilesProcessLogics;

[TestClass()]
public class EwsdFilesProcessLogicTests
{
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly Queue<EwsdFileTask> _ewsdFileTaskListNew = new();
    private List<EwsdFileTask>? _ewsdFileTaskListProcessed;

    [TestInitialize]
    public void Setup()
    {
        // Runs before each test. (Optional)
        _cancellationTokenSource = new();
        _ewsdFileTaskListProcessed = new();
    }

    /*[TestCleanup]
    public void TearDown()
    {
        // Runs after each test. (Optional)
    }*/

    [TestMethod()]
    public async Task EwsdFilesProcessLogic_ProcessFilesAsync_Test()
    {
        _ewsdFileTaskListNew.Enqueue(
            new EwsdFileTask
            {
                File = new EwsdFile
                {
                    Name = "validFileName",
                    Directory = "path/",
                    Path = "path/validFileName"
                },
                Status = EwsdFileProcessStatus.New,
                Message = null
            });

        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IEwsdFileParsingLogic>()
            .Setup(x => x.ParseFileAsync(It.IsAny<EwsdFileTask>(), It.IsAny<CancellationToken>()))
            .Returns((EwsdFileTask ewsdFileTask, CancellationToken _) =>
            {
                ewsdFileTask.Status = EwsdFileProcessStatus.Processed;

                return Task.CompletedTask;
            });

        mock
            .Mock<IProcessEwsdFileTaskManager>()
            .Setup(x => x.GetNew())
            .Returns(() =>
            {
                if (_ewsdFileTaskListNew.TryDequeue(out var ewsdFileTask))
                {
                    return ewsdFileTask;
                }

                return null;
            });

        mock
            .Mock<IProcessEwsdFileTaskManager>()
            .Setup(x => x.Save(It.IsAny<EwsdFileTask>()))
            .Callback((EwsdFileTask ewsdFileTask) =>
            {
                _ewsdFileTaskListProcessed?.Add(ewsdFileTask);
            });

        var sut = mock.Create<EwsdFilesProcessLogic>();

        CancelProcessFilesAsync();

        // Act
        try
        {
            await sut.ProcessFilesAsync(_cancellationTokenSource!.Token);
        }
        catch (TaskCanceledException) { }

        // Assert
        Assert.AreEqual(1, _ewsdFileTaskListProcessed?.Count);

        // Отменить обработку ewsd файлов
        async void CancelProcessFilesAsync()
        {
            await Task.Delay(2000);
            _cancellationTokenSource!.Cancel();
        }
    }

    [TestMethod()]
    public async Task EwsdFilesProcessLogic_ProcessFilesAsync_Status_Test()
    {
        _cancellationTokenSource = new();

        _ewsdFileTaskListNew.Enqueue(
        new EwsdFileTask
        {
            File = new EwsdFile
            {
                Name = "validFileName",
                Directory = "path/",
                Path = "path/validFileName"
            },
            Status = EwsdFileProcessStatus.New,
            Message = null
        });

        _ewsdFileTaskListNew.Enqueue(
        new EwsdFileTask
        {
            File = new EwsdFile
            {
                Name = "inValidFileName",
                Directory = "path/",
                Path = "path/inValidFileName"
            },
            Status = EwsdFileProcessStatus.New,
            Message = null
        });

        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IEwsdFileParsingLogic>()
            .Setup(x => x.ParseFileAsync(It.IsAny<EwsdFileTask>(), It.IsAny<CancellationToken>()))
            .Returns((EwsdFileTask ewsdFileTask, CancellationToken _) =>
            {
                if (ewsdFileTask.File?.Name == "validFileName") ewsdFileTask.Status = EwsdFileProcessStatus.Processed;
                if (ewsdFileTask.File?.Name == "inValidFileName") ewsdFileTask.Status = EwsdFileProcessStatus.Error;

                return Task.CompletedTask;
            });

        mock
            .Mock<IProcessEwsdFileTaskManager>()
            .Setup(x => x.GetNew())
            .Returns(() => _ewsdFileTaskListNew.TryDequeue(out var ewsdFileTask) ? ewsdFileTask : null);

        mock
            .Mock<IProcessEwsdFileTaskManager>()
            .Setup(x => x.Save(It.IsAny<EwsdFileTask>()))
            .Callback((EwsdFileTask ewsdFileTask) =>
            {
                _ewsdFileTaskListProcessed?.Add(ewsdFileTask);
            });

        var sut = mock.Create<EwsdFilesProcessLogic>();

        CancelProcessFilesAsync();

        // Act
        try
        {
            await sut.ProcessFilesAsync(_cancellationTokenSource.Token);
        }
        catch (TaskCanceledException) { }

        // Assert
        Assert.AreEqual(2, _ewsdFileTaskListProcessed?.Count);
        Assert.AreEqual(EwsdFileProcessStatus.Processed, _ewsdFileTaskListProcessed?.First().Status);
        Assert.AreEqual(EwsdFileProcessStatus.Error, _ewsdFileTaskListProcessed?.Last().Status);
        
        //Отменить обработку ewsd файлов
        async void CancelProcessFilesAsync()
        {
            await Task.Delay(3000);
            _cancellationTokenSource!.Cancel();
        }
    }
}