using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(sqlException);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                                    this.studentValidatorService
                                       .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                retrieveStudentById.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            var exception = new Exception();

            var failedStudentServiceException =
                new FailedStudentServiceException(exception);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                                    this.studentValidatorService
                                        .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                retrieveStudentById.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentServiceException))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
