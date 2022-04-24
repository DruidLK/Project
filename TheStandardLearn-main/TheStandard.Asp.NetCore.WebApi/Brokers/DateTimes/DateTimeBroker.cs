using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetDateTime() =>
            DateTimeOffset.UtcNow;
    }
}
