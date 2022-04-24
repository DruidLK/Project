using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions
{
    public class StudentProcessingDependencyException : Exception
    {
        public StudentProcessingDependencyException(Exception innerException)
            : base("A student Processing dependency Occured, Contact Support.",
                    innerException)
        { }
    }
}
