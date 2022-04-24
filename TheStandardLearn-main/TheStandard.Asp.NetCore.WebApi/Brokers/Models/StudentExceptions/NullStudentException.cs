using Xeptions;

namespace TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions
{
    public class NullStudentException : Xeption
    {
        public NullStudentException()
            : base(message: "Student is null.")
        { }
    }
}
