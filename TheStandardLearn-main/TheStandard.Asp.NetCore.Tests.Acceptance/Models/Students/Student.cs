using System;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.Models.Students
{
    public class Student
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public GenderView GenderView { get; set; }
        public string IdentityNumber { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
