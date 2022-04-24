using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentViewServiceTests
{
    public partial class StudentViewServiceTest
    {
        [Fact]
        public async Task ShouldRegisterStudentOnRegisterWhenPassedIn()
        {
            // Arrange - Given
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid currentlyLoggedInUser = Guid.NewGuid();

            dynamic randomStudent =
                CreateRandomProperties(
                theDate: dateTime,
                theUser: currentlyLoggedInUser);

            Student inputStudent = new()
            {
                Id = randomStudent.Id,
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                Gender = randomStudent.Gender,
                CreatedDate = randomStudent.CreatedDate,
                UpdatedDate = randomStudent.UpdatedDate,
                CreatedBy = randomStudent.CreatedBy,
                UpdatedBy = randomStudent.UpdatedBy
            };
            Student retrievedStudent = inputStudent;

            StudentView inputStudentView = new()
            {
                Id = randomStudent.Id,
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = randomStudent.GenderView,
            };
            StudentView expectedStudentView = inputStudentView;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(service =>
                service.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedInUser);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(
                    It.Is(SameStudentAs(
                        inputStudent))))
                            .ReturnsAsync(retrievedStudent);

            // Act - When
            StudentView actualStudentView =
                await this.studentViewService
                    .RegisterStudentAsync(inputStudentView);

            // Assert - Then
            actualStudentView.Should().BeEquivalentTo(expectedStudentView);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Once);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Once);

            this.studentProcessingServiceMock.Verify(service =>
                service.UpsertStudentAsync(
                    It.Is(SameStudentAs(
                        inputStudent))),
                            Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentByIdOnRetrieveWhenPassedIn()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;

            StudentView expectedStudentView = new()
            {
                Id = randomStudent.Id,
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = (GenderView)randomStudent.Gender,
                BirthDate = randomStudent.BirthDate
            };
            Guid studentId = inputStudent.Id;

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            StudentView actualStudentView =
                await this.studentViewService
                    .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            actualStudentView.Should().BeEquivalentTo(expectedStudentView);

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveStudentsOnRetrieveAllWhenCalled()
        {
            // Arrange - Given
            List<dynamic> randomList = CreateRandomList();

            List<Student> ListOfStudents =
                randomList.SelectMany<dynamic, Student>(student => student)
                    .ToList();

            IQueryable<Student> retrieveStudents =
                ListOfStudents.AsQueryable();

            List<StudentView> ListOfStudentsView =
                ListOfStudents.Select(property =>
                     new StudentView
                     {
                         Id = property.Id,
                         UserId = property.UserId,
                         IdentityNumber = property.IdentityNumber,
                         FirstName = property.FirstName,
                         MiddleName = property.MiddleName,
                         LastName = property.LastName,
                         GenderView = (GenderView)property.Gender,
                         BirthDate = property.BirthDate
                     }).ToList();

            IQueryable<StudentView> expectedStudentsView =
                ListOfStudentsView.AsQueryable();

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveStudentsAsync())
                    .Returns(retrieveStudents);

            // Act - When
            IEnumerable<StudentView> listOfStudentsView =
                this.studentViewService
                        .RetrieveStudents();

            // Assert - Then
            listOfStudentsView.Should().BeEquivalentTo(expectedStudentsView);

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveStudentsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Never);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Never);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldUpdateStudentOnModifyWhenPassedIn()
        {
            // Arrange - Given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = GetDateTimeNow();
            Guid theUser = Guid.NewGuid();
            Guid currentlyLoggedIn = Guid.NewGuid();

            dynamic randomProperties =
                CreateRandomProperties(
                    createdDate: randomDateTime,
                    updatedDate: randomDateTime,
                    createdBy: theUser,
                    loggedIn: theUser);


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
            Student inputStudent = new()
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
                UpdatedDate = dateTime,
                CreatedBy = randomProperties.CreatedBy,
                UpdatedBy = currentlyLoggedIn
            };
            Student retrievedStudent = inputStudent;

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
            StudentView expectedStudentView = inputStudentView;

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveMatchingStudentByIdAsync(storageStudent.Id))
                    .ReturnsAsync(storageStudent);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTime())
                    .Returns(dateTime);

            this.userManagmentBrokerMock.Setup(broker =>
                broker.GetCurrentlyLoggedIn())
                    .Returns(currentlyLoggedIn);

            this.studentProcessingServiceMock.Setup(service =>
                service.UpsertStudentAsync(
                    It.Is(SameStudentAs(
                        inputStudent))))
                            .ReturnsAsync(retrievedStudent);

            // Act - When
            StudentView actualStudentView =
                 await this.studentViewService
                      .ModifyStudentAsync(inputStudentView);

            // Assert - Then
            actualStudentView.Should().BeEquivalentTo(expectedStudentView);

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveMatchingStudentByIdAsync(storageStudent.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Once);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Once);

            this.studentProcessingServiceMock.Verify(service =>
                service.UpsertStudentAsync(
                    It.Is(SameStudentAs(
                        inputStudent))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemoveStudentOnTryRemoveWhenPassedIn()
        {
            // Arrange - Given
            dynamic randomStudent = CreateRandomStudent();

            Student retrievedStudent = randomStudent;
            StudentView expectedStudent = new()
            {
                Id = randomStudent.Id,
                UserId = randomStudent.UserId,
                IdentityNumber = randomStudent.IdentityNumber,
                FirstName = randomStudent.FirstName,
                MiddleName = randomStudent.MiddleName,
                LastName = randomStudent.LastName,
                GenderView = (GenderView)randomStudent.Gender,
                BirthDate = randomStudent.BirthDate
            };
            Guid studentId = retrievedStudent.Id;

            this.studentProcessingServiceMock.Setup(service =>
                service.TryRemoveStudentAsync(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            StudentView actualStudent =
                await this.studentViewService
                    .RemoveStudentAsync(studentId);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentProcessingServiceMock.Verify(service =>
                service.TryRemoveStudentAsync(studentId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTime(),
                    Times.Never);

            this.userManagmentBrokerMock.Verify(broker =>
                broker.GetCurrentlyLoggedIn(),
                    Times.Never);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
