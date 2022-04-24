using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        [Theory]
        [MemberData(nameof(GetValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnTryRemoveWhenValidationExceptionOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentProcessingDependencyValidationException =
                new StudentProcessingDependencyValidationException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<Student> tryRemove =
                this.studentProcessingService
                        .TryRemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyValidationException>(() =>
                tryRemove.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingDependencyValidationException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnTryRemoveIfDependencyOrServiceExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentProcessingDependencyException =
                new StudentProcessingDependencyException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<Student> tryRemove =
                this.studentProcessingService
                        .TryRemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyException>(() =>
                tryRemove.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingDependencyException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnTryRemoveWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            var exception = new Exception();

            var expectedStudentProcessingServiceException =
                new StudentProcessingServiceException(exception);

            this.studentValidatorServiceMock.Setup(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<Student> tryRemove =
                this.studentProcessingService
                        .TryRemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingServiceException>(() =>
                tryRemove.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingServiceException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
