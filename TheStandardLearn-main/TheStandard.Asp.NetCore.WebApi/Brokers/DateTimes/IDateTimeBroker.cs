using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetDateTime();
    }
}
