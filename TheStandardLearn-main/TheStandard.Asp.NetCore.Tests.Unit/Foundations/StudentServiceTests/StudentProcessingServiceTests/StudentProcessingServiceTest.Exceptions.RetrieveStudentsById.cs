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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfValidationOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentProcessingDependencyValidationException =
                new StudentProcessingDependencyValidationException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                this.studentProcessingService
                        .RetrieveMatchingStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyValidationException>(() =>
                retrieveStudentById.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyServiceExceptionsOccursAndLogItAsync(
          Exception DependencyExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentProcessingDependencyException =
                new StudentProcessingDependencyException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                this.studentProcessingService
                        .RetrieveMatchingStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyException>(() =>
                retrieveStudentById.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            Exception exception = new();

            var expectedStudentProcessingServiceException =
                new StudentProcessingServiceException(exception);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                this.studentProcessingService
                        .RetrieveMatchingStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingServiceException>(() =>
                retrieveStudentById.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()),
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
