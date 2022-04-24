using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices
{
    public partial class StudentViewService
    {
        private delegate ValueTask<StudentView> registerRemoveStudentFunc();
        private delegate IQueryable<StudentView> retrieveAllFunc();
        private delegate ValueTask<Student> modifyStudentFunc();

        private async ValueTask<StudentView> TryCatch(registerRemoveStudentFunc registerRemoveStudentfunc)
        {
            try
            {
                return await registerRemoveStudentfunc();
            }
            catch (StudentValidationException studentValidationException)
            {
                throw CreateAndLogStudentViewDependenyValidationException(studentValidationException);
            }
            catch (StudentProcessingDependencyValidationException studentProcessingDependencyValidationException)
            {
                throw CreateAndLogStudentViewDependenyValidationException(studentProcessingDependencyValidationException);
            }
            catch (StudentProcessingDependencyException studentProcessingDependencyException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingDependencyException);
            }
            catch (StudentProcessingServiceException studentProcessingServiceException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingServiceException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogStudentViewServiceException(exception);
            }
        }
        private IQueryable<StudentView> TryCatch(retrieveAllFunc retrieveallFunc)
        {
            try
            {
                return retrieveallFunc();
            }
            catch (StudentProcessingDependencyException studentProcessingDependencyException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingDependencyException);
            }
            catch (StudentProcessingServiceException studentProcessingServiceException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingServiceException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogStudentViewServiceException(exception);
            }
        }
        private async ValueTask<Student> TryCatch(modifyStudentFunc modifystudentFunc)
        {
            try
            {
                return await modifystudentFunc();
            }
            catch (StudentValidationException studentValidationException)
            {
                throw CreateAndLogStudentViewDependenyValidationException(studentValidationException);
            }
            catch (StudentProcessingDependencyValidationException studentProcessingDependencyValidationException)
            {
                throw CreateAndLogStudentViewDependenyValidationException(studentProcessingDependencyValidationException);
            }
            catch (StudentProcessingDependencyException studentProcessingDependencyException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingDependencyException);
            }
            catch (StudentProcessingServiceException studentProcessingServiceException)
            {
                throw CreateAndLogStudentviewDependencyException(studentProcessingServiceException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogStudentViewServiceException(exception);
            }
        }
        private StudentViewDependencyValidationException CreateAndLogStudentViewDependenyValidationException(
            Exception exception)
        {
            Exception innerException =
                exception.InnerException;

            var studentViewDependencyValidationException =
                new StudentViewDependencyValidationException(innerException);

            this.loggingBroker.LogError(studentViewDependencyValidationException);

            return studentViewDependencyValidationException;
        }

        private StudentViewDependencyException CreateAndLogStudentviewDependencyException(
            Exception exception)
        {
            Exception innerException =
                exception.InnerException;

            var studentViewDependencyException =
                new StudentViewDependencyException(innerException);

            this.loggingBroker.LogError(studentViewDependencyException);

            return studentViewDependencyException;
        }

        private StudentViewServiceException CreateAndLogStudentViewServiceException(
            Exception exception)
        {
            var studentViewServiceException =
                new StudentViewServiceException(exception);

            this.loggingBroker.LogError(studentViewServiceException);

            return studentViewServiceException;
        }
    }
}
