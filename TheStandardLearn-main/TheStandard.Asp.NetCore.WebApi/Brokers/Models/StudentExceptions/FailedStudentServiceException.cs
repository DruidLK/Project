using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class FailedStudentServiceException : Exception
    {
        public FailedStudentServiceException(Exception innerException)
            : base("Service Exception has Occured, Contact Support.",
                  innerException)
        { }
    }
}
