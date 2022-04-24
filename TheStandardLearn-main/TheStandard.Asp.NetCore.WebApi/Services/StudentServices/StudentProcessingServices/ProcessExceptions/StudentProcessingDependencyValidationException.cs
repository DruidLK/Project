using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions
{
    public class StudentProcessingDependencyValidationException : Exception
    {
        public StudentProcessingDependencyValidationException(Exception innerException)
            : base("Processing dependency Validations occured, Try again", innerException)
        { }
    }
}
