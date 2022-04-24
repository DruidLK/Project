using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices
{
    public partial class StudentValidatorService : IStudentValidatorService
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IStorageBroker storageBroker;

        public StudentValidatorService(
            ILoggingBroker loggingBroker,
            IStorageBroker storageBroker)
        {
            this.loggingBroker = loggingBroker;
            this.storageBroker = storageBroker;
        }

        public async ValueTask<Student> AddStudentAsync(Student student) =>
            await TryCatch(async () =>
            {
                ValidateOnAdd(student);

                return await this.storageBroker.InsertStudentAsync(student);

            });
        public IQueryable<Student> RetrieveAllStudents() =>
            TryCatch(() =>
           {
               return this.storageBroker.SelectStudents();
           });
        public async ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
            await TryCatch(async () =>
            {
                ValidateOnRetrieve(studentId);

                Student storageStudent =
                    await this.storageBroker.SelectStudentById(studentId);

                ValidateStorageOnRetrieve(storageStudent, studentId);

                return storageStudent;
            });
        public async ValueTask<Student> ModifyStudentAsync(Student student) =>

            await TryCatch(async () =>
            {
                ValidateOnModify(student);

                Student storageStudent =
                         await this.storageBroker
                            .SelectStudentById(student.Id);

                ValidateStorageOnModify(storageStudent, student);

                return await this.storageBroker
                    .UpdateStudentAsync(student);
            });
        public async ValueTask<Student> RemoveStudentAsync(Guid studentId) =>
            await TryCatch(async () =>
            {
                ValidateOnRemove(studentId);

                Student storageStudent =
                     await this.storageBroker
                         .SelectStudentById(studentId);

                ValidateStorageOnRemove(storageStudent, studentId);

                return await this.storageBroker
                            .DeleteStudentAsync(storageStudent);
            });
    }
}
