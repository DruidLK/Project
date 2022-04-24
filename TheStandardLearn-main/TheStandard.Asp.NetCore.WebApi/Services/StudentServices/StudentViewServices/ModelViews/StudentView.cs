using System;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews
{
    public class StudentView
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public GenderView GenderView { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
