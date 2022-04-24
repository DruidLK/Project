using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentViewServiceTests
{
    public partial class StudentViewServiceTest
    {
        [Theory]
        [MemberData(nameof(GetValidationExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfValidationExceptionsOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();

            dynamic randomStudent =
                CreateRandomProperties(
                    theDate: dateTime,
                    theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = randomStudent.GenderView,
            };

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentViewDependencyValidationException =
                new StudentViewDependencyValidationException(innerException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(broker =>
                broker.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<StudentView> registerStudent =
                this.studentViewService
                        .RegisterStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyValidationException>(() =>
                registerStudent.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Once);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Once);

            this.studentProcessingServiceMock.Verify(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()),
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
        public async Task ShouldThrowDependencyExceptionOnRegisterIfDependenyExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();

            dynamic randomStudent =
               CreateRandomProperties(
                   theDate: dateTime,
                   theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = randomStudent.GenderView,
            };

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(service =>
                service.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<StudentView> registerStudent =
                this.studentViewService
                        .RegisterStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyException>(() =>
                registerStudent.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Once);

            this.userManagmentBrokerMock.Verify(service =>
                service.GetCurrentlyLoggedIn(),
                    Times.Once);

            this.studentProcessingServiceMock.Verify(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyException))),
                            Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRegisterIfAnExceptionsOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();
            Exception exception = new();

            dynamic randomStudent =
                CreateRandomProperties(
                    theDate: dateTime,
                    theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = randomStudent.GenderView,
            };

            var expectedStudentViewServiceException =
                new StudentViewServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(service =>
                service.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<StudentView> registerStudent =
                this.studentViewService
                        .RegisterStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewServiceException>(() =>
                registerStudent.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Once);

            this.userManagmentBrokerMock.Verify(service =>
                service.GetCurrentlyLoggedIn(),
                    Times.Once);

            this.studentProcessingServiceMock.Verify(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewServiceException))),
                            Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
