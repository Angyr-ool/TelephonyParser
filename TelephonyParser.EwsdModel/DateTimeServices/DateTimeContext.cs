namespace TelephonyParser.EwsdModel.DateTimeServices;

public class DateTimeContext : IDateTimeContext
{
    public DateTime GetDate()
    {
        return DateTime.Now;
    }
}
