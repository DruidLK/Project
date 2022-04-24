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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidandLogIt()
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
            ValueTask<Student> retrieveStudentByIdAsync =
                this.studentProcessingService
                        .RetrieveMatchingStudentByIdAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                retrieveStudentByIdAsync.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }
    }
}
