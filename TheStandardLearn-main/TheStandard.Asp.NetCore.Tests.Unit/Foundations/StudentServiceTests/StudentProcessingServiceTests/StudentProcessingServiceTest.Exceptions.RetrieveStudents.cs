using System;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentProcessingServiceTests
{
    public partial class StudentProcessingServiceTest
    {
        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveStudentsWhenExceptionsOccursAndLogIt(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentProcessingDependencyException =
                new StudentProcessingDependencyException(innerException);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Throws(DependencyExceptions);

            // Act - When / Assert - Then
            Assert.Throws<StudentProcessingDependencyException>(() =>
               this.studentProcessingService
                        .RetrieveStudentsAsync());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
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
        public void ShouldThrowServiceExceptionOnRetrieveStudentsWhenAnExceptionsOccursAndLogIt()
        {
            // Arrange - Given
            Exception exception = new();

            var expectedStudentProcessingServiceException =
                new StudentProcessingServiceException(exception);

            this.studentValidatorServiceMock.Setup(service =>
                service.RetrieveAllStudents())
                    .Throws(exception);

            // Act - When / Assert - Then
            Assert.Throws<StudentProcessingServiceException>(() =>
                this.studentProcessingService.RetrieveStudentsAsync());

            this.studentValidatorServiceMock.Verify(service =>
                service.RetrieveAllStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentProcessingServiceException))),
                            Times.Once);

            this.studentValidatorServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
