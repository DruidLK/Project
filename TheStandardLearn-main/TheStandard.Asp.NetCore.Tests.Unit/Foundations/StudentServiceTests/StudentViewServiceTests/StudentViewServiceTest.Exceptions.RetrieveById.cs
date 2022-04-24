using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentViewServiceTests
{
    public partial class StudentViewServiceTest
    {
        [Theory]
        [MemberData(nameof(GetValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveByIdWhenValidationExceptionsOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentViewDependencyValidationException =
                new StudentViewDependencyValidationException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<StudentView> retrieveStudentById =
                this.studentViewService
                        .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyValidationException>(() =>
                retrieveStudentById.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyValidationException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDependencyExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<StudentView> retrieveById =
                this.studentViewService
                        .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyException>(() =>
                retrieveById.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            Exception exception = new();

            var expectedStudentViewServiceException =
                new StudentViewServiceException(exception);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<StudentView> retrieveById =
                this.studentViewService
                        .RetrieveStudentByIdAsync(studentId);

            // Assert - Then 
            await Assert.ThrowsAsync<StudentViewServiceException>(() =>
                retrieveById.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewServiceException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }
    }
}
