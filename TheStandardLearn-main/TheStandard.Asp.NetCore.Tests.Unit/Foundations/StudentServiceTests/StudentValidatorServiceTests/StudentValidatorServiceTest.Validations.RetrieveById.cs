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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenGuidIsInvalidAndLogItAsync()
        {
            // Arrange - Given
            Guid invalidGuid = Guid.Empty;

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.Id),
                values: "Id is Required.");

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // Act - When
            ValueTask<Student> retrieveById =
                this.studentValidatorService
                    .RetrieveStudentByIdAsync(invalidGuid);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                retrieveById.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStudentDoesNotExistAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            Student emptyStudent = null;

            var studentNotFoundException =
                new StudentNotFoundException(studentId);

            var expectedStudentValidationException =
                new StudentValidationException(studentNotFoundException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(emptyStudent);

            // Act - When
            ValueTask<Student> retrieveStudentById =
                                        this.studentValidatorService
                                            .RetrieveStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                retrieveStudentById.AsTask());

            this.storageBrokerMock.Verify(broker =>
                 broker.SelectStudentById(studentId),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(expectedStudentValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
