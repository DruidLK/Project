using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class StudentValidationException : Exception
    {
        public StudentValidationException(Exception innerException)
            : base(message: "Student validation errors occured.",
                  innerException)
        { }
    }
}
