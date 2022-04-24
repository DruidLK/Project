using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        private readonly Mock<IStudentValidatorService> studentValidatorServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentProcessingService studentProcessingService;

        public StudentProcessingServiceTest()
        {
            this.studentValidatorServiceMock = new Mock<IStudentValidatorService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.studentProcessingService = new StudentProcessingService(
                studentValidatorService: this.studentValidatorServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static int GetRandomNumbers() =>
            new IntRange(min: 2, max: 15).GetValue();
        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumbers()).GetValue();
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(new DateTime()).GetValue();
        private static IQueryable<Student> CreateRandomListOFStudents() =>
            CreateRandomFiller().Create(count: GetRandomNumbers()).AsQueryable();
        private static IQueryable<Student> CreateRandomStudents(Student student)
        {
            List<Student> students =
                CreateRandomListOFStudents().ToList();

            students.Add(student);

            return students.AsQueryable();
        }
        private static Exception GetInnerException() =>
            new(GetRandomString());
        public static TheoryData GetValidationExceptions() =>
            new TheoryData<Exception>()
            {
               new StudentValidationException(GetInnerException())
            };
        public static TheoryData GetDependencyExceptions()
        {
            Exception innerException = GetInnerException();

            return new TheoryData<Exception>()
            {
                new StudentDependencyException(innerException),
                new StudentServiceException(innerException)
            };
        }
        private static Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                    && actualException.InnerException.Message == expectedException.InnerException.Message;
        }
        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(
            Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                    && actualException.InnerException.Message == expectedException.InnerException.Message
                      && (actualException.InnerException as Xeption)
                             .DataEquals(expectedException.InnerException.Data);
        }
        private static Student CreateRandomStudent() =>
                CreateRandomFiller().Create();
        private static IQueryable<Student> CreateRandomListOfStudents(int randomCount) =>
             CreateRandomFiller().Create(randomCount).AsQueryable();
        private static Filler<Student> CreateRandomFiller()
        {
            var student = new Filler<Student>();

            student.Setup()
                  .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return student;
        }
    }
}
