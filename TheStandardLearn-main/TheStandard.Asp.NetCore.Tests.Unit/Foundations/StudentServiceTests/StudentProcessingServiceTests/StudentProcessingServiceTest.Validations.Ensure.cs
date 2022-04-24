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
        public async Task ShouldThrowValidationExceptionOnEnsureStudentWhenStudentIsNullAndLogItAsync()
        {
            // Arrange -  Given
            Student nullStudent = null;

            var nullStudentException =
                new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            // Act - When
            ValueTask<Student> ensureStudent =
                this.studentProcessingService
                         .EnsureStudentExistAsync(nullStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                ensureStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                     It.Is(SameExceptionAs(
                           expectedStudentValidationException))),
                                Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Never);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnEnsureStudentWhenIdIsInvalidAndLogItAsync()
        {
            // Arrange - Given
            var invalidStudent = new Student();

            var invalidStudentException =
                new InvalidStudentException();

            invalidStudentException.AddData(
                key: nameof(Student.Id),
                values: "Id is Required.");

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // Act - When
            ValueTask<Student> ensureStudent =
                this.studentProcessingService
                        .EnsureStudentExistAsync(invalidStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                ensureStudent.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameValidationExceptionAs(
                        expectedStudentValidationException))),
                            Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Never);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.studentValidatorServiceMock.VerifyNoOtherCalls();
        }
    }
}
