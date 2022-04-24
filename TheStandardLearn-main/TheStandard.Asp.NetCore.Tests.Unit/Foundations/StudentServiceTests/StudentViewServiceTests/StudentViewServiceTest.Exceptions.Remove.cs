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
        public async Task ShouldThrowDependencyValidationOnRemoveWhenValidationExceptionsOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentViewDependencyValidationException =
               new StudentViewDependencyValidationException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<StudentView> removeStudent =
                this.studentViewService
                        .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyValidationException>(() =>
                removeStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyValidationException))),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Never);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Never);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public async Task ShouldThrowDependencyOnRemoveWhenDependencyExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<StudentView> removeStudent =
                this.studentViewService
                        .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyException>(() =>
                removeStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyException))),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Never);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Never);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThowServiceExceptionOnRemoveWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();

            Exception exception = new();

            var expectedStudentViewServiceException =
                new StudentViewServiceException(exception);

            this.studentProcessingServiceMock.Setup(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<StudentView> removeStudent =
                this.studentViewService
                        .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewServiceException>(() =>
                removeStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.TryRemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewServiceException))),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Never);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Never);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }
    }
}
