using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IStudentValidatorService studentValidatorService;

        public StudentValidatorServiceTest()
        {
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.studentValidatorService = new StudentValidatorService(
                loggingBroker: this.loggingBrokerMock.Object,
                storageBroker: this.storageBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
            FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

        private static IQueryable<Student> GetListOfStudent()
        {
            var date = GetRandomDateTime();

            return new List<Student>
            {
                CreateRandomStudent(date),
                CreateRandomStudent(date),
                CreateRandomStudent(date)
            }.AsQueryable();
        }
        private static Expression<Func<Exception, bool>> SameExceptionAs(
           Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                    && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Student CreateRandomStudent() =>
            CreateRandomFiller().Create();

        private static Student CreateRandomStudent(DateTimeOffset date) =>
            CreateRandomFiller(date).Create();
        private static int GetRandomMinutes() =>
            new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeNumbers() =>
            GetRandomMinutes() * -1;

        public static TheoryData RandomMinutes()
        {
            return new TheoryData<int>
            {
                GetRandomMinutes(),
                GetNegativeNumbers()
            };
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

        private static Filler<Student> CreateRandomFiller()
        {
            var student = new Filler<Student>();

            student.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return student;
        }
        private static Filler<Student> CreateRandomFiller(DateTimeOffset date)
        {
            var student = new Filler<Student>();

            student.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return student;
        }
    }
}
