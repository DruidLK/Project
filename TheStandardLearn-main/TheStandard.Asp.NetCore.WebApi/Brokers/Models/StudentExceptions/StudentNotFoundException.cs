using System;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(Guid studentId)
            : base($"The student with {studentId} was not found. try again.")
        { }
    }
}
