using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        [Fact]
        public async Task ShouldRetrieveStudentOnEnsureWhenIsFoundAsync()
        {
            // Arrange -  Given
            int randomNumberOfStudents = GetRandomNumbers();

            Random random = new();
            int randomIndex = random.Next(randomNumberOfStudents);

            IQueryable<Student> randomListOfStudent =
                CreateRandomListOfStudents(randomNumberOfStudents);

            IQueryable<Student> listOfStudents = randomListOfStudent;

            Student inputStudent = randomListOfStudent.ElementAt(randomIndex);
            Student expectedStudent = inputStudent;

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(listOfStudents);

            // Act - When
            Student actualStudent =
                    await this.studentProcessingService
                            .EnsureStudentExistAsync(inputStudent);

            // Assert - then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddStudentOnEnsureWhenNotFoundAsync()
        {
            // Arrange - Given
            int randomNumberOfStudents = GetRandomNumbers();

            IQueryable<Student> randomListOfStudents =
                CreateRandomListOfStudents(randomNumberOfStudents);

            IQueryable<Student> retrievedListOfStudents = randomListOfStudents;

            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Student expectedStudent = inputStudent;

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedListOfStudents);

            this.studentValidatorServiceMock.Setup(service =>
                service.AddStudentAsync(inputStudent))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            Student actualStudent =
                await this.studentProcessingService
                      .EnsureStudentExistAsync(inputStudent);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(inputStudent),
                    Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddStudentOnUpsertIfNotExistAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student addedStudent = inputStudent;
            Student expectedStudent = inputStudent.DeepClone();

            IQueryable<Student> randomListOfStudents =
                CreateRandomListOFStudents();

            IQueryable<Student> retrievedStudents =
                randomListOfStudents;

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedStudents);

            this.studentValidatorServiceMock.Setup(service =>
                service.AddStudentAsync(inputStudent))
                    .ReturnsAsync(addedStudent);

            // Act - When
            Student actualStudent =
                await this.studentProcessingService
                    .UpsertStudentAsync(inputStudent);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.ModifyStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveStudentsOnRetrieveWhenCalled()
        {
            // Arrange - Given
            IQueryable<Student> randomListOfStudents = CreateRandomListOFStudents();
            IQueryable<Student> retrievedStudents = randomListOfStudents;
            IEnumerable<Student> expectedStudents = randomListOfStudents;

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedStudents);

            // Act - When
            IEnumerable<Student> actualStudent =
                this.studentProcessingService
                        .RetrieveStudentsAsync();

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudents);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentByIdOnMatchingWhenPassedIn()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student retrievedStudent = randomStudent;
            Student expectedStudent = retrievedStudent;
            Guid studentId = retrievedStudent.Id;


            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveStudentByIdAsync(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            Student actualStudent =
                await this.studentProcessingService
                    .RetrieveMatchingStudentByIdAsync(studentId);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyStudentOnUpsertIfSudentExistAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student modifyedStudent = inputStudent;
            Student expectedStudent = modifyedStudent.DeepClone();

            IQueryable<Student> randomStudents =
                CreateRandomStudents(inputStudent);

            IQueryable<Student> retrievedStudents =
                randomStudents;

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedStudents);

            this.studentValidatorServiceMock.Setup(service =>
                service.ModifyStudentAsync(inputStudent))
                    .ReturnsAsync(modifyedStudent);

            // Act - When
            Student actualStudent =
                await this.studentProcessingService
                    .UpsertStudentAsync(inputStudent);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.ModifyStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldTryRemoveStudentWhenPassedIn()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Student expectedStudent = inputStudent;
            Guid studentId = inputStudent.Id;

            this.studentValidatorServiceMock.Setup(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            Student actualStudent =
                await this.studentProcessingService
                    .TryRemoveStudentAsync(studentId);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.studentValidatorServiceMock.Verify(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }
    }
}
