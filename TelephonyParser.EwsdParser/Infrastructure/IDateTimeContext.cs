namespace TelephonyParser.EwsdParser.Infrastructure;


public interface IDateTimeContext
{
    DateTime Get();
}

public class DateTimeContext : IDateTimeContext
{
    public DateTime Get()
    {
        return DateTime.Now;
    }
}