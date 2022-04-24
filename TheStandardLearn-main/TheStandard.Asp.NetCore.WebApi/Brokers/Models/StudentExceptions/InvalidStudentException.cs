using System;
using System.Collections;
using Xeptions;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class InvalidStudentException : Xeption
    {
        public InvalidStudentException()
            : base(message: "Invalid student. Please fix the errors and try again.")
        { }

        public InvalidStudentException(Exception innerException, IDictionary data)
            : base(message: "Invalid student. Please fix the errors and try again.",
                  innerException,
                    data)
        { }
    }
}
