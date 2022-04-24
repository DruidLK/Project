using System;
using Moq;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Unit.Foundations.StudentServiceTests.StudentViewServiceTests
{
    public partial class StudentViewServiceTest
    {
        [Theory]
        [MemberData(nameof(GetDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyExceptionsOccursAndLogIt(
            Exception DependencyExceptions)
        {
            // Arrange - Given
            Exception innerException =
                DependencyExceptions.InnerException;

            var expectedStudentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveStudentsAsync())
                    .Throws(DependencyExceptions);


            // Act - When / Assert - Then
            Assert.Throws<StudentViewDependencyException>(() =>
                this.studentViewService.RetrieveStudents());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveStudentsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewDependencyException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAnExceptionOccursAndLogIt()
        {
            // Arrange - Given
            Exception exception = new();

            var expectedStudentViewServiceException =
                new StudentViewServiceException(exception);

            this.studentProcessingServiceMock.Setup(service =>
                service.RetrieveStudentsAsync())
                    .Throws(exception);

            // Act - When / Assert - Then
            Assert.Throws<StudentViewServiceException>(() =>
                this.studentViewService.RetrieveStudents());

            this.studentProcessingServiceMock.Verify(service =>
                service.RetrieveStudentsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(
                    It.Is(SameExceptionAs(
                        expectedStudentViewServiceException))),
                            Times.Once);

            this.studentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagmentBrokerMock.VerifyNoOtherCalls();
        }
    }
}
