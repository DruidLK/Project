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
        public async Task ShouldThrowValidationExceptionOnRemoveWhenNullndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.Empty;

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.Id),
                values: "Id is Required.");

            var expectedStudentValidationException =
                    new StudentValidationException(invalidStudentException);

            // Act - When
            ValueTask<Student> removeStudent =
                            this.studentValidatorService
                                .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                removeStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStudentNotFoundAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            Student retrievedStudent = null;

            var studentNotFoundException =
                new StudentNotFoundException(studentId);

            var expectedStudentValidationException =
                new StudentValidationException(studentNotFoundException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(retrievedStudent);

            // Act - When
            ValueTask<Student> removeStudent =
                        this.studentValidatorService
                            .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                removeStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);


            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
