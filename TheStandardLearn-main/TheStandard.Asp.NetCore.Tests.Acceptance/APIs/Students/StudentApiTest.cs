using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.Tests.Acceptance.Brokers.ApiBrokers;
using TheStandard.Asp.NetCore.Tests.Acceptance.Models.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.APIs.Students
{
    [Collection(nameof(ApiBrokerCollection))]
    public partial class StudentApiTest
    {
        private readonly ApiBrokerTest apiBrokerTest;

        public StudentApiTest(ApiBrokerTest apiBrokerTest) =>
            this.apiBrokerTest = apiBrokerTest;

        public async ValueTask PostStudents(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                await this.apiBrokerTest.PostStudent(student);
            }
        }

        public async ValueTask DeleteStudents(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                await this.apiBrokerTest.DeleteStudent(student.Id);
            }
        }
        private static Student CreateRandomStudent() =>
            CreateRandomFiller().Create();
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 12).GetValue();
        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(new DateTime()).GetValue();
        private static List<Student> GetRandomStudentsList() =>
            CreateRandomFiller().Create(GetRandomNumber()) as List<Student>;
        private static GenderView GetRandomGender()
        {
            int randomGender =
                new IntRange(min: 0, max: 0).GetValue();

            return (GenderView)randomGender;
        }
        private static Filler<Student> CreateRandomFiller()
        {
            Filler<Student> student = new();

            student.Setup()
                .OnProperty(property => property.Id).IgnoreIt()
                .OnProperty(property => property.GenderView).Use(GetRandomGender())
                .OnType<DateTimeOffset>().Use(GetRandomDate());

            return student;
        }

    }
}
