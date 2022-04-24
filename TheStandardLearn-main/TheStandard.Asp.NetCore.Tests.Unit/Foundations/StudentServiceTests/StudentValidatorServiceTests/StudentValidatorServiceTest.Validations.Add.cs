using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddWhenNullStudentAndLogItAsync()
        {
            //Arrange - Given
            Student nullStudent = null;

            var nullStudentException =
                new NullStudentException();

            var expectedstudentValidationException =
                new StudentValidationException(nullStudentException);

            //Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(nullStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                      addStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameValidationExceptionAs(expectedstudentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenInputsAreInvalidAndLogItAsync(
            string invalidInputs)
        {
            //Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();

            var invalidStudent = new Student
            {
                UserId = invalidInputs,
                IdentityNumber = invalidInputs,
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

            //Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(invalidStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                addStudent.AsTask());


            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                        It.Is(SameValidationExceptionAs(
                            expectedStudentValidationException))),
                                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddWhenCreatedDateNotSameAsUpdatedDateAndLogItAsync()
        {
            //Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            inputStudent.UpdatedDate = GetRandomDateTime();

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.UpdatedDate),
                values: "The Dates Should Be Equal.");

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            //Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(inputStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                addStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(
                       It.Is(SameValidationExceptionAs(
                           expectedStudentValidationException))),
                               Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
