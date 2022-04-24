using System;
using System.Threading.Tasks;
using Force.DeepCloner;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyWhenSqlExceptionOnSelectOccursAndLogItAsync()
        {
            //Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Guid studentId = inputStudent.Id;
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(sqlException);

            // Act - When
            ValueTask<Student> modifyStudent =
                    this.studentValidatorService
                        .ModifyStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(
                        It.Is(SameExceptionAs(
                            expectedStudentDependencyException))),
                                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyWhenSqlExceptionOnUpdateOccursAndLogItAsync()
        {
            //Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Guid studentId = inputStudent.Id;
            var sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(inputStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentAsync(inputStudent))
                    .ThrowsAsync(sqlException);

            //Act - When
            ValueTask<Student> modifyStudent =
                    this.studentValidatorService
                        .ModifyStudentAsync(inputStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(inputStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(
                        It.Is(SameExceptionAs(
                            expectedStudentDependencyException))),
                                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            //Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent.DeepClone();
            Guid studentId = inputStudent.Id;

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedStudentDependencyException =
                new LockedStudentDependencyException(dbUpdateConcurrencyException);

            var expectedStudentDependencyException =
                new StudentDependencyException(lockedStudentDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            //Act - When
            ValueTask<Student> modifyStudent =
                    this.studentValidatorService
                        .ModifyStudentAsync(inputStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(inputStudent),
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
        public async Task ShouldThrowDependencyExceptionOnModifyWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            //Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent.DeepClone();
            Guid studentId = inputStudent.Id;
            var dbUpdateException = new DbUpdateException();

            var failedStudentStorageException =
                new FailedStudentStorageException(dbUpdateException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentAsync(inputStudent))
                    .ThrowsAsync(dbUpdateException);

            //Act - When
            ValueTask<Student> modifyStudent =
                        this.studentValidatorService
                            .ModifyStudentAsync(inputStudent);

            //Assert - Then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(inputStudent),
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
        public async Task ShouldThrowServiceExceptionOnModifyWhenAnExceptionOccursAndLogItAsync()
        {
            // Arrange - Given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Guid studentId = inputStudent.Id;
            var exception = new Exception();

            var failedStudentServiceException =
                    new FailedStudentServiceException(exception);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentById(studentId))
                    .ThrowsAsync(exception);

            // Act - When
            ValueTask<Student> modifyStudent =
                           this.studentValidatorService
                                  .ModifyStudentAsync(inputStudent);

            // Assert - Then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                modifyStudent.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentById(studentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
