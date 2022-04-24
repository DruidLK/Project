using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class StudentDependencyException : Exception
    {
        public StudentDependencyException(Exception innerException)
            : base("Dependecy Exception Occured.",
                    innerException)
        { }
    }
}
