using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions
{
    public class StudentViewDependencyValidationException : Exception
    {
        public StudentViewDependencyValidationException(Exception innerException)
            : base("View dependency Validations occured, Try again", innerException)
        { }
    }
}
