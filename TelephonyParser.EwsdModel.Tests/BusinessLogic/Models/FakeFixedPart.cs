using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

public class FakeFixedPart : IEwsdPackage
{
    public byte StartByte => throw new NotImplementedException();

    public object Clone()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public byte GetLength(in byte[] record, int startBytePositionInRecord)
    {
        throw new NotImplementedException();
    }
}