using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.DateTimes;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using TheStandard.Asp.NetCore.WebApi.Brokers.UserManagment;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using Tynamix.ObjectFiller;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentViewServiceTests
{
    public partial class StudentViewServiceTest
    {
        private readonly Mock<IStudentProcessingService> studentProcessingServiceMock;
        private readonly Mock<IUserManagmentBroker> userManagmentBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentViewService studentViewService;
        private readonly ICompareLogic compareLogic;

        public StudentViewServiceTest()
        {
            this.studentProcessingServiceMock = new Mock<IStudentProcessingService>();
            this.userManagmentBrokerMock = new Mock<IUserManagmentBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();


            var compareConfig = new ComparisonConfig();
            compareConfig.IgnoreProperty<Student>(student => student.Id);

            this.compareLogic = new CompareLogic(compareConfig);

            this.studentViewService = new StudentViewService(
                studentProcessingService: this.studentProcessingServiceMock.Object,
                userManagmentBroker: this.userManagmentBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetDateTimeNow() =>
            DateTimeOffset.UtcNow;
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
        private static string GetFirstName() =>
            new RealNames(NameStyle.FirstName).GetValue();
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
        private static Exception GetInnerException() =>
            new(GetRandomString());
        public static TheoryData GetValidationExceptions()
        {
            Exception innerException = GetInnerException();

            return new TheoryData<Exception>
            {
                new StudentValidationException(innerException),
                new StudentProcessingDependencyValidationException(innerException)
            };
        }
        public static TheoryData GetDependencyExceptions()
        {
            Exception innerException = GetInnerException();

            return new TheoryData<Exception>
            {
               new StudentProcessingDependencyException(innerException),
               new StudentProcessingServiceException(innerException)
            };
        }
        private static Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                   && actualException.InnerException.Message == expectedException.InnerException.Message;
        }
        private static List<dynamic> CreateRandomList()
        {
            List<dynamic> students = new();

            students.Add(CreateRandomFiller().Create(GetRandomNumber()));

            return students;
        }
        private static dynamic CreateRandomStudent() =>
            CreateRandomFiller().Create();
        private static string GetLastName() =>
            new RealNames(NameStyle.LastName).GetValue();
        private static GenderView GetRandomGender()
        {
            int genderCount =
                Enum.GetValues(typeof(GenderView)).Length;

            int randomGender =
                new IntRange(min: 0, max: genderCount).GetValue();

            return (GenderView)randomGender;
        }
        private Expression<Func<Student, bool>> SameStudentAs(
            Student expectedStudent)
        {
            return actualStudent =>
                this.compareLogic
                  .Compare(expectedStudent, actualStudent)
                      .AreEqual;
        }
        private static Filler<Student> CreateRandomFiller()
        {
            var student = new Filler<Student>();

            student.Setup()
                .OnType<DateTimeOffset>().Use(GetDateTimeNow());

            return student;

        }
        private static dynamic CreateRandomProperties(
            DateTimeOffset theDate,
            Guid theUser)
        {
            GenderView randomGender = GetRandomGender();
            DateTimeOffset randomDate = GetRandomDateTime();

            return new
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString(),
                IdentityNumber = GetRandomString(),
                FirstName = GetFirstName(),
                MiddleName = GetFirstName(),
                LastName = GetLastName(),
                Gender = (Gender)randomGender,
                GenderView = randomGender,
                BirthDate = randomDate,
                CreatedDate = theDate,
                UpdatedDate = theDate,
                CreatedBy = theUser,
                UpdatedBy = theUser
            };
        }

        private static dynamic CreateRandomProperties(
            DateTimeOffset createdDate,
            DateTimeOffset updatedDate,
            Guid createdBy,
            Guid loggedIn)
        {
            GenderView randomGender = GetRandomGender();
            DateTimeOffset randomDate = GetRandomDateTime();

            return new
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString(),
                IdentityNumber = GetRandomString(),
                FirstName = GetFirstName(),
                MiddleName = GetFirstName(),
                LastName = GetLastName(),
                Gender = (Gender)randomGender,
                GenderView = randomGender,
                BirthDate = randomDate,
                CreatedDate = createdDate,
                UpdatedDate = updatedDate,
                CreatedBy = createdBy,
                UpdatedBy = loggedIn
            };

        }
    }
}
