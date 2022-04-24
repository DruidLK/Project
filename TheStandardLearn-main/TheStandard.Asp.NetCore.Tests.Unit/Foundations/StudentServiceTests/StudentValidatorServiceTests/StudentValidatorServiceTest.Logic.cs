using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public async Task ShouldAddStudentWhenPassedIn()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Student expectedStudent = inputStudent;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            Student actualStudent =
                 await this.studentValidatorService
                        .AddStudentAsync(inputStudent);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(actualStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllStudentsWhenPassedIn()
        {
            // Arrange - Given
            IQueryable<Student> randomListOfStudent = GetListOfStudent();
            IQueryable<Student> inputStudent = randomListOfStudent;
            IQueryable<Student> retrievedStudent = inputStudent;
            IQueryable<Student> expectedStudent = inputStudent;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudents())
                    .Returns(retrievedStudent);

            // Act - When
            IQueryable<Student> actualStudent =
                                 this.studentValidatorService
                                       .RetrieveAllStudents();

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudents(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentByIdWhenPassedIn()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Student expectedStudent = inputStudent;
            Guid studentId = inputStudent.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            Student actualStudent =
                await this.studentValidatorService
                    .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyStudentWhenPassedIn()
        {
            // Arrange - Given
            int randomDays = GetRandomMinutes();
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent.DeepClone();
            inputStudent.UpdatedDate = inputStudent.UpdatedDate.AddDays(randomDays);
            Student retrievedStudent = inputStudent;
            Student expectedStudent = inputStudent;
            Guid studentId = inputStudent.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentAsync(inputStudent))
                    .ReturnsAsync(inputStudent);

            // Act - When
            Student actualStudent =
                await this.studentValidatorService
                    .ModifyStudentAsync(inputStudent);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(inputStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemoveStudentWhenPassedIn()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Student expectedStudent = retrievedStudent;
            Guid studentId = inputStudent.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(retrievedStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAsync(inputStudent))
                    .ReturnsAsync(expectedStudent);

            // Act - When
            Student actualStudent =
                        await this.studentValidatorService
                            .RemoveStudentAsync(studentId);

            // Assert - Then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(inputStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
