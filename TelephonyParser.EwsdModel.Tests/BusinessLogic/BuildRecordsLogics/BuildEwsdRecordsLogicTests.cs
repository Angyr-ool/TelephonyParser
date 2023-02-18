using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelephonyParser.EwsdModel.BusinessLogic.BuildPackagesLogics;
using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordLogics;
using TelephonyParser.EwsdModel.BusinessLogic.BuildRecordsLogics;
using TelephonyParser.EwsdModel.BusinessLogic.Models;
using TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.BuildRecordsLogics;

[TestClass()]
public class BuildEwsdRecordsLogicTests
{
    private byte[][]? _recordBytesArray;

    /// <summary>
    /// Этот метод запускается перед вызовом каждого тестового метода
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        _recordBytesArray = new byte[][] { Array.Empty<byte>() };
    }

    [TestMethod()]
    public void BuildEwsdRecordsLogic_BuildRecords_Test()
    {
        using var mock = AutoMock.GetLoose();
        // Arrange - configure the mock
        mock
            .Mock<IBuildPackagesLogic>()
            .Setup(x => x.Build(It.IsAny<byte[]>()))
            .Returns((byte[] _) => new IRecordPackage[] { new FakeFixedPart() });

        mock
            .Mock<IBuildRecordLogic>()
            .Setup(x => x.Build(It.IsAny<IRecordPackage[]>()))
            .Returns((IRecordPackage[] _) => new FakeEwsdRecord());

        var sut = mock.Create<BuildEwsdRecordsLogic>();

        // Act
        var records = sut.BuildRecords(_recordBytesArray!);

        // Assert
        Assert.AreEqual(1, records.Length);
    }
}