using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentValidatorServiceTests
{
    public partial class StudentValidatorServiceTest
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ThrowsAsync(sqlException);

            // Act - When
            ValueTask<Student> addStudent =
                       this.studentValidatorService
                              .AddStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                addStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(
                        It.Is(SameExceptionAs(
                            expectedStudentDependencyException))),
                                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedStudentDependencyException =
                new LockedStudentDependencyException(dbUpdateConcurrencyException);

            var expectedStudentDependencyException =
                new StudentDependencyException(lockedStudentDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                addStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                           expectedStudentDependencyException))),
                                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            var dbUpdateException = new DbUpdateException();

            var failedStudentStorageDependencyException =
                new FailedStudentStorageException(dbUpdateException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateException);

            // Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                addStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentDependencyException))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            DateTimeOffset randomDate = GetRandomDateTime();
            Student randomStudent = CreateRandomStudent(randomDate);
            Student inputStudent = randomStudent;
            var exception = new Exception();

            var failedStudentServiceException =
                new FailedStudentServiceException(exception);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<Student> addStudent =
                this.studentValidatorService
                    .AddStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                addStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentServiceException))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
