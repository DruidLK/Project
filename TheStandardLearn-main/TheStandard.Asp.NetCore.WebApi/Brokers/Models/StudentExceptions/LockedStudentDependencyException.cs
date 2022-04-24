using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class LockedStudentDependencyException : Exception
    {
        public LockedStudentDependencyException(Exception innerException)
            : base("Locked Student Exception Occured. Contact Support.",
                    innerException)
        { }
    }
}
