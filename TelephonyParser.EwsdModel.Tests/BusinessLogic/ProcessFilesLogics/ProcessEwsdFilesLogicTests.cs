using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;
using TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;
using TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.ProcessFilesLogics;

[TestClass()]
public class ProcessEwsdFilesLogicTests
{
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Задачи на обработку
    /// </summary>
    private readonly Queue<IEwsdFileTask> _ewsdFileTaskListNew = new();

    /// <summary>
    /// Задачи в обработке
    /// </summary>
    private List<IEwsdFileTask>? _ewsdFileTaskListOnProcess;

    /// <summary>
    /// Задачи прошедшие обработку
    /// </summary>
    private List<IEwsdFileTask>? _ewsdFileTaskListProcessed;

    /// <summary>
    /// Задачи с ошибкой
    /// </summary>
    private List<IEwsdFileTask>? _ewsdFileTaskListError;

    [TestInitialize]
    public void Setup()
    {
        // Runs before each test. (Optional)
        _cancellationTokenSource = new();
        _ewsdFileTaskListOnProcess = new();
        _ewsdFileTaskListProcessed = new();
        _ewsdFileTaskListError = new();
    }

    [TestMethod()]
    public async Task ProcessEwsdFilesLogic_ProcessFilesAsync_Test()
    {
        _ewsdFileTaskListNew.Enqueue(new FakeEwsdFileTask
        {
            File = new FakeEwsdFile
            {
                Name = "validFileName",
                Directory = "path/",
                Path = "path/validFileName"
            },
            Status = EwsdFileTaskStatus.New,
            Message = null
        });

        using var mock = AutoMock.GetLoose();

        // Arrange - configure the mock
        mock
            .Mock<IParseEwsdFileLogic>()
            .Setup(x => x.ParseFileAsync(It.IsAny<IEwsdFileTask>(), It.IsAny<CancellationToken>()))
            .Returns((IEwsdFileTask ewsdFileTask, CancellationToken _) =>
            {
                ewsdFileTask.Status = EwsdFileTaskStatus.Processed;
                return Task.CompletedTask;
            });

        mock
            .Mock<IEwsdExternalResourceService>()
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
            .Mock<IEwsdExternalResourceService>()
            .Setup(x => x.Save(It.IsAny<IEwsdFileTask>()))
            .Callback((IEwsdFileTask ewsdFileTask) =>
            {
                switch (ewsdFileTask.Status)
                {
                    case EwsdFileTaskStatus.OnProcess:
                        _ewsdFileTaskListOnProcess?.Add(ewsdFileTask);
                        break;
                    case EwsdFileTaskStatus.Processed:
                        _ewsdFileTaskListProcessed?.Add(ewsdFileTask);
                        break;
                    default:
                        _ewsdFileTaskListError?.Add(ewsdFileTask);
                        break;
                }
            });

        var sut = mock.Create<ProcessEwsdFilesLogic>();
        sut.ProcessFilesNotify += Sut_ProcessFilesNotify;
        sut.ProcessFilesStatusChanged += Sut_ProcessFilesStatusChanged;

        // Act
        try
        {
            await sut.ProcessFilesAsync(_cancellationTokenSource!.Token);
        }
        catch (TaskCanceledException) { }

        // Assert
        Assert.AreEqual(1, _ewsdFileTaskListOnProcess?.Count);
        Assert.AreEqual(1, _ewsdFileTaskListProcessed?.Count);
        Assert.AreEqual(0, _ewsdFileTaskListError?.Count);
    }

    private readonly List<(ProcessFilesNotifyType NotifyType, string Message)> _notifies = new();

    private void Sut_ProcessFilesNotify(ProcessFilesEventArgs eventArgs)
    {
        _notifies.Add(new (eventArgs.NotifyType, eventArgs.Message));
    }

    private void Sut_ProcessFilesStatusChanged(ProcessFilesStatus processFilesStatus)
    {
        if (processFilesStatus == ProcessFilesStatus.FileProcessed)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}