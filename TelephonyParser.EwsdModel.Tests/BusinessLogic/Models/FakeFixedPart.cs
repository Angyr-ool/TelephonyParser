using TelephonyParser.EwsdModel.BusinessLogic.Models;

namespace TelephonyParser.EwsdModel.Tests.BusinessLogic.Models;

public class FakeFixedPart : IRecordPackage
{
    public byte Number => throw new NotImplementedException();

    public int Length => throw new NotImplementedException();
}