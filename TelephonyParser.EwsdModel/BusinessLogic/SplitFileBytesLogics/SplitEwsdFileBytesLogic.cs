namespace TelephonyParser.EwsdModel.BusinessLogic.SplitFileBytesLogics;

public class SplitEwsdFileBytesLogic : ISplitEwsdFileBytesLogic
{
    public byte[][] SplitBytes(byte[] bytes)
    {
        try
        {
            List<byte[]> recordsInBytes = new();

            var i = 0;
            var recordFinish = 0;

            var fileBytes = bytes.ToList();

            foreach (var b in fileBytes)
            {
                if (b == 0x84 && i >= recordFinish)
                {
                    var recordCount = fileBytes[i + 1] + fileBytes[i + 2];

                    recordFinish = i + recordCount;

                    recordsInBytes.Add(fileBytes.GetRange(i, recordCount).ToArray());
                }

                i++;
            }

            return recordsInBytes.ToArray();
        }
        catch (Exception e)
        {
            throw new Exception($"Ewsd bytes splitting error: {e.Message}");
        }
    }
}
