using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class StudentServiceException : Exception
    {
        public StudentServiceException(Exception innerException)
            : base("Student Service Exception has occured.",
                  innerException)
        { }
    }
}
