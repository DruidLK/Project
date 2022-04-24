using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices
{
    public partial class StudentValidatorService
    {
        private delegate ValueTask<Student> AddRetrieveModifyRemove();
        private delegate IQueryable<Student> RetrieveAll();

        private async ValueTask<Student> TryCatch(AddRetrieveModifyRemove addRetrieveModifyRemove)
        {
            try
            {
                return await addRetrieveModifyRemove();
            }
            catch (SqlException sqlException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedStudentDependencyException =
                     new LockedStudentDependencyException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedStudentDependencyException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedStudentStorageException);
            }
            catch (NullStudentException nullStudentException)
            {
                throw CreateAndLogValidationException(nullStudentException);
            }
            catch (InvalidStudentException invalidStudentException)
            {
                throw CreateAndLogValidationException(invalidStudentException);
            }
            catch (StudentNotFoundException studentNotFoundException)
            {
                throw CreateAndLogValidationException(studentNotFoundException);
            }
            catch (Exception exception)
            {
                var failedStudentServiceException =
                    new FailedStudentServiceException(exception);

                throw CreateAndLogServiceException(failedStudentServiceException);
            }
        }

        private IQueryable<Student> TryCatch(RetrieveAll retrieveAll)
        {
            try
            {
                return retrieveAll();
            }
            catch (SqlException sqlException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
            }
            catch (Exception exception)
            {
                var failedStudentServiceException =
                    new FailedStudentServiceException(exception);

                throw CreateAndLogServiceException(failedStudentServiceException);
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

        private StudentDependencyException CreateAndLogCriticalDependencyException(
            Exception exception)
        {
            var studentDependencyException =
                new StudentDependencyException(exception);

            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private StudentDependencyException CreateAndLogDependencyException(
            Exception exception)
        {
            var studentDependencyException =
                new StudentDependencyException(exception);

            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private StudentServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var studentServiceException =
                new StudentServiceException(exception);

            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
