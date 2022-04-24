using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class FailedStudentStorageException : Exception
    {
        public FailedStudentStorageException(Exception innerException)
            : base("Storage Exception Occured. Contact Support.",
                  innerException)
        { }
    }
}
