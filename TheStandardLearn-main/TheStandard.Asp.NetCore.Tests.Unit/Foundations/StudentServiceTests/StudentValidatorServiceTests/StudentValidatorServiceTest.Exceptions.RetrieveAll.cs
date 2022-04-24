using System;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlOccursAndLogItAsync()
        {
            // Arrange - Given
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudents())
                    .Throws(sqlException);

            // Act - When / Assert - Then
            Assert.Throws<StudentDependencyException>(() =>
                this.studentValidatorService
                    .RetrieveAllStudents());

            this.storageBrokerMock.Verify(Broker =>
                Broker.SelectStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(
                    It.Is(SameExceptionAs(
                        expectedStudentDependencyException))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            var exception = new Exception();

            var failedStudentServiceException =
                new FailedStudentServiceException(exception);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudents())
                    .Throws(exception);

            // Act - When - Assert - Then
            Assert.Throws<StudentServiceException>(() =>
                this.studentValidatorService
                    .RetrieveAllStudents());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentServiceException))),
                            Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
