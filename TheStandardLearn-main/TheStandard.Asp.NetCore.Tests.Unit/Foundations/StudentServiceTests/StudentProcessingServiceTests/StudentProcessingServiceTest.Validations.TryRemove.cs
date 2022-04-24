using System;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnTryRemoveWhenInvalidIdAndLogItAync()
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
            ValueTask<Student> tryRemove =
                this.studentProcessingService
                        .TryRemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                tryRemove.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.RemoveStudentAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
