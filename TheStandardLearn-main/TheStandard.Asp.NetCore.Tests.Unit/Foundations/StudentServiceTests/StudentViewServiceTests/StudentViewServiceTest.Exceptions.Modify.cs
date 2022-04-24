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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfValidationExceptionsOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();

            dynamic randomProperties =
                CreateRandomProperties(
                    theDate: dateTime,
                    theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                GenderView = randomProperties.GenderView,
                BirthDate = randomProperties.BirthDate
            };

            Student storageStudent = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                Gender = randomProperties.Gender,
                BirthDate = randomProperties.BirthDate,
                CreatedDate = randomProperties.CreatedDate,
                UpdatedDate = randomProperties.UpdatedDate,
                CreatedBy = randomProperties.CreatedBy,
                UpdatedBy = randomProperties.UpdatedBy
            };

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentViewDependencyValidationException =
                new StudentViewDependencyValidationException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageStudent);

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
            ValueTask<StudentView> modifyStudent =
                this.studentViewService
                        .ModifyStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyValidationException>(() =>
                modifyStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                 service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();

            dynamic randomProperties =
              CreateRandomProperties(
                  theDate: dateTime,
                  theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                GenderView = randomProperties.GenderView,
                BirthDate = randomProperties.BirthDate
            };

            Student storageStudent = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                Gender = randomProperties.Gender,
                BirthDate = randomProperties.BirthDate,
                CreatedDate = randomProperties.CreatedDate,
                UpdatedDate = randomProperties.UpdatedDate,
                CreatedBy = randomProperties.CreatedBy,
                UpdatedBy = randomProperties.UpdatedBy
            };

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageStudent);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(broker =>
                broker.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<StudentView> modifyStudent =
                this.studentViewService
                        .ModifyStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewDependencyException>(() =>
                modifyStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

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
                        expectedStudentViewDependencyException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedIn = Guid.NewGuid();


            dynamic randomProperties =
              CreateRandomProperties(
                  theDate: dateTime,
                  theUser: currentlyLoggedIn);

            StudentView inputStudentView = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                GenderView = randomProperties.GenderView,
                BirthDate = randomProperties.BirthDate
            };

            Student storageStudent = new()
            {
                Id = randomProperties.Id,
                UserId = randomProperties.UserId,
                IdentityNumber = randomProperties.IdentityNumber,
                FirstName = randomProperties.FirstName,
                MiddleName = randomProperties.MiddleName,
                LastName = randomProperties.LastName,
                Gender = randomProperties.Gender,
                BirthDate = randomProperties.BirthDate,
                CreatedDate = randomProperties.CreatedDate,
                UpdatedDate = randomProperties.UpdatedDate,
                CreatedBy = randomProperties.CreatedBy,
                UpdatedBy = randomProperties.UpdatedBy
            };

            Exception exception = new();

            var expectedStudentViewServiceException =
                new StudentViewServiceException(exception);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageStudent);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(broker =>
                broker.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<StudentView> modifyStudent =
                this.studentViewService
                        .ModifyStudentAsync(inputStudentView);

            // Assert - Then
            await Assert.ThrowsAsync<StudentViewServiceException>(() =>
                modifyStudent.AsTask());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

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
                            expectedStudentViewServiceException))),
                                Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
