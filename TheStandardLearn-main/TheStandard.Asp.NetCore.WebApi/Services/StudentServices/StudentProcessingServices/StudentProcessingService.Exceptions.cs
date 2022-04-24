using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices.ProcessExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices
{
    public partial class StudentProcessingService
    {
        private delegate ValueTask<Student> EnsureUpsertRemoveRetrieve();
        private delegate IQueryable<Student> RetrieveStudents();

        private IQueryable<Student> TryCatch(RetrieveStudents retrieveStudents)
        {
            try
            {
                return retrieveStudents();
            }
            catch (StudentDependencyException studentDependencyException)
            {
                throw CreateAndLogProcessingDependencyException(studentDependencyException);
            }
            catch (StudentServiceException studentServiceException)
            {
                throw CreateAndLogProcessingDependencyException(studentServiceException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogProcessingServiceException(exception);
            }
        }

        private async ValueTask<Student> TryCatch(EnsureUpsertRemoveRetrieve ensureUpsertRemoveRetrieve)
        {
            try
            {
                return await ensureUpsertRemoveRetrieve();
            }
            catch (NullStudentException nullStudentException)
            {
                throw CreateAndLogValidationException(nullStudentException);
            }
            catch (InvalidStudentException invalidStudentException)
            {
                throw CreateAndLogValidationException(invalidStudentException);
            }
            catch (StudentValidationException studentValidationException)
            {
                throw CreateAndLogProcessingDependencyValidationException(studentValidationException);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                throw CreateAndLogProcessingDependencyException(studentDependencyException);
            }
            catch (StudentServiceException studentServiceException)
            {
                throw CreateAndLogProcessingDependencyException(studentServiceException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogProcessingServiceException(exception);
            }
        }

        private StudentValidationException CreateAndLogValidationException(
            Exception exception)
        {
            var studentValidationException =
                new StudentValidationException(exception);

            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }

        private StudentProcessingDependencyValidationException CreateAndLogProcessingDependencyValidationException(
            Exception exception)
        {
            Exception innerException =
                exception.InnerException;

            var studentProcessingDependencyValidationException =
                new StudentProcessingDependencyValidationException(innerException);

            this.loggingBroker.LogError(studentProcessingDependencyValidationException);

            return studentProcessingDependencyValidationException;
        }

        private StudentProcessingDependencyException CreateAndLogProcessingDependencyException(
            Exception exception)
        {
            Exception innerException =
                exception.InnerException;

            var studentProcessingDependencyException =
                new StudentProcessingDependencyException(innerException);

            this.loggingBroker.LogError(studentProcessingDependencyException);

            return studentProcessingDependencyException;
        }

        private StudentProcessingServiceException CreateAndLogProcessingServiceException(
            Exception exception)
        {
            var studentProcessingServiceException =
                new StudentProcessingServiceException(exception);

            this.loggingBroker.LogError(studentProcessingServiceException);

            return studentProcessingServiceException;
        }
    }
}
