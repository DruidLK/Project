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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // Arrange - Given 
            Guid studentId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(sqlException);

            // Act - When
            ValueTask<Student> removeStudent =
                        this.studentValidatorService
                            .RemoveStudentAsync(studentId);

            // Assert - then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                removeStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(
                    It.Is(SameExceptionAs(
                        expectedStudentDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyOccursAndLogItAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student retrievedStudent = inputStudent;
            Guid studentId = inputStudent.Id;

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedStudentDependencyException =
                new LockedStudentDependencyException(dbUpdateConcurrencyException);

            var expectedStudentDependencyException =
                new StudentDependencyException(lockedStudentDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(retrievedStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // Act - When
            ValueTask<Student> removeStudent =
                    this.studentValidatorService
                           .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                removeStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(inputStudent),
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
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student returnedStudent = inputStudent;
            Guid studentId = inputStudent.Id;

            var dbUpdateException =
                new DbUpdateException();

            var failedStudentStorageException =
                new FailedStudentStorageException(dbUpdateException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(returnedStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateException);

            // Act - When
            ValueTask<Student> removeStudent =
                this.studentValidatorService
                    .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                removeStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(inputStudent),
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
        public async Task ShouldThrowServiceExceptionOnRemoveWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Guid studentId = Guid.NewGuid();
            var exception = new Exception();

            var failedStudentServiceException =
                 new FailedStudentServiceException(exception);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(exception);

            // Act - when
            ValueTask<Student> removeStudent =
                this.studentValidatorService
                    .RemoveStudentAsync(studentId);

            // Assert - Then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                removeStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                     It.Is(SameExceptionAs(
                         expectedStudentServiceException))),
                                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
