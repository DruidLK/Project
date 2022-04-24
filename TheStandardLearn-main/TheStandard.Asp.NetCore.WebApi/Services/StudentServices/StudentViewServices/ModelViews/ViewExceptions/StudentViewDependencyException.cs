using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions
{
    public class StudentViewDependencyException : Exception
    {
        public StudentViewDependencyException(Exception innerException)
           : base("A student View dependency Occured, Contact Support.",
                   innerException)
        { }
    }
}
