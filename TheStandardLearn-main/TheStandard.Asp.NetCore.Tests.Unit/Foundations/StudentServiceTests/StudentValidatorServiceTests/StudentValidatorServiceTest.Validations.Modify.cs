using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentIsNullAndLogItAsync()
        {
            // Arrange - Given
            Student nullStudent = null;

            var nullStudentException =
                new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            // Act - When
            ValueTask<Student> modifyStudent =
                                this.studentValidatorService
                                    .ModifyStudentAsync(nullStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenInvalidInputsAndLogItAsync(
            string invalidInputs)
        {
            // Arrange - Given
            Student invalidStudent = new()
            {
                IdentityNumber = invalidInputs,
                UserId = invalidInputs,
                FirstName = invalidInputs,
                MiddleName = invalidInputs,
                LastName = invalidInputs
            };

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.Id),
                values: "Id is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.UserId),
                values: "Text is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.IdentityNumber),
                values: "Text is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.FirstName),
                values: "Text is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.MiddleName),
                values: "Text is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.LastName),
                values: "Text is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.BirthDate),
                values: "Date is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.CreatedDate),
                values: "Date is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.UpdatedDate),
                values: "Date is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.CreatedBy),
                values: "Id is Required.");

            invalidStudentException.AddData(
                key: nameof(Student.UpdatedBy),
                values: "Id is Required.");

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // Act - When
            ValueTask<Student> modifyStudent =
                                    this.studentValidatorService
                                        .ModifyStudentAsync(invalidStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentNotFoundAndLogItAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student nullStudent = null;
            Guid studentId = inputStudent.Id;

            var studentNotFoundException =
                new StudentNotFoundException(studentId);

            var expectedStudentValidationException =
                new StudentValidationException(studentNotFoundException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(nullStudent);

            // Act - When
            ValueTask<Student> modifyStudent =
                            this.studentValidatorService
                                .ModifyStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                        It.Is(SameExceptionAs(
                            expectedStudentValidationException))),
                                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsNotSameAsStorageAndLogItAsync()
        {
            // Arrange - Given
            int randomDays = GetRandomMinutes();
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent.DeepClone();
            Guid studentId = inputStudent.Id;

            inputStudent.CreatedDate =
                inputStudent.CreatedDate.AddDays(randomDays);

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.CreatedDate),
                values: "The Dates Should Be Equal.");

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            ValueTask<Student> modifyStudent =
                    this.studentValidatorService
                        .ModifyStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
