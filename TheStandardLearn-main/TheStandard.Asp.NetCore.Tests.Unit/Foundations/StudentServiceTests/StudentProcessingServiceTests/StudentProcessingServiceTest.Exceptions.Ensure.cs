using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        [Theory]
        [MemberData(nameof(GetValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnEnsureWhenValidationExceptionsOccursAndLogItAsync(
            Exception ValidationExceptions)
        {
            // Arrange - Given
            int randomNumberOfStudents = GetRandomNumbers();

            IQueryable<Student> randomListOfStudents =
                CreateRandomListOfStudents(randomNumberOfStudents);

            IQueryable<Student> retrievedStudents = randomListOfStudents;

            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;

            Exception innerException =
                ValidationExceptions.InnerException;

            var expectedStudentProcessingDependencyValidationException =
                new StudentProcessingDependencyValidationException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedStudents);

            this.studentValidatorServiceMock.Setup(service =>
                service.AddStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(ValidationExceptions);

            // Act - When
            ValueTask<Student> ensureStudent =
                             this.studentProcessingService
                                    .EnsureStudentExistAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyValidationException>(() =>
                ensureStudent.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingDependencyValidationException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnEnsureWhenDependencyAndServiceExceptionsOccursAndLogItAsync(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            int randomNumberOfStudents = GetRandomNumbers();

            IQueryable<Student> randomListOfStudents =
                CreateRandomListOfStudents(randomNumberOfStudents);

            IQueryable<Student> retrievedStudents = randomListOfStudents;

            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;

            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentProcessingDependencyException =
                new StudentProcessingDependencyException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Returns(retrievedStudents);

            this.studentValidatorServiceMock.Setup(service =>
                service.AddStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(DependencyExceptions);

            // Act - When
            ValueTask<Student> ensureStudent =
                this.studentProcessingService
                         .EnsureStudentExistAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingDependencyException>(() =>
                ensureStudent.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingDependencyException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnEnsureWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;

            var exception = new Exception();

            var expectedStudentProcessingServiceException =
                new StudentProcessingServiceException(exception);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Throws(exception);

            // Act - When
            ValueTask<Student> ensureStudent =
                this.studentProcessingService
                        .EnsureStudentExistAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentProcessingServiceException>(() =>
                ensureStudent.AsTask());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingServiceException))),
                            Times.Once);

            this.studentValidatorServiceMock.Verify(service =>
                service.AddStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
