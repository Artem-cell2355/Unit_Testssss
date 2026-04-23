namespace AspNetExample.Shared;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetDateTime() => DateTime.UtcNow;
}