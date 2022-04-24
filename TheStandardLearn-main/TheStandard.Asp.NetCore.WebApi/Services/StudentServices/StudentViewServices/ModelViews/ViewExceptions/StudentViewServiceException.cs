using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions
{
    public class StudentViewServiceException : Exception
    {
        public StudentViewServiceException(Exception innerException)
            : base("A student View exception occured, contact support.",
                    innerException)
        { }
    }
}
