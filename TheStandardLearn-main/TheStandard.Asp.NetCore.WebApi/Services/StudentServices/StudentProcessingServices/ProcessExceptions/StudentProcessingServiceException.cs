using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions
{
    public class StudentProcessingServiceException : Exception
    {
        public StudentProcessingServiceException(Exception innerException)
            : base("A student Processing Exception occured, contact support.",
                    innerException)
        { }
    }
}
