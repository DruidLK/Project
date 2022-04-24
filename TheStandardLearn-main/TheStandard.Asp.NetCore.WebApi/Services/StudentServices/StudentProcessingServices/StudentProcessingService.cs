using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices
{
    public partial class StudentProcessingService : IStudentProcessingService
    {
        private readonly IStudentValidatorService studentValidatorService;
        private readonly ILoggingBroker loggingBroker;

        public StudentProcessingService(
            IStudentValidatorService studentValidatorService,
            ILoggingBroker loggingBroker)
        {
            this.studentValidatorService = studentValidatorService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Student> EnsureStudentExistAsync(Student student) =>
            await TryCatch(async () =>
            {

                ValidateOnEnsure(student);

                IQueryable<Student> storageStudent =
                         this.studentValidatorService
                                  .RetrieveAllStudents();

                Student maybeStudent =
                    storageStudent.
                        FirstOrDefault(retrievedStudentBanana =>
                              retrievedStudentBanana.Id == student.Id);

                return maybeStudent switch
                {
                    { } => maybeStudent,
                    _ => await this.studentValidatorService.AddStudentAsync(student)
                };

            });
        public IQueryable<Student> RetrieveStudentsAsync() =>
            TryCatch(() =>
        {
            IQueryable<Student> students =
                this.studentValidatorService
                     .RetrieveAllStudents();

            return students;

        });
        public async ValueTask<Student> RetrieveMatchingStudentByIdAsync(Guid studentId) =>
            await TryCatch(async () =>
            {
                ValidateOnRetrieveStudentById(studentId);

                return await
                     this.studentValidatorService
                          .RetrieveStudentByIdAsync(studentId);
            });
        public async ValueTask<Student> UpsertStudentAsync(Student student) =>
            await TryCatch(async () =>
           {
               ValidateOnUpsert(student);

               bool storageStudent =
                     this.studentValidatorService
                            .RetrieveAllStudents()
                                 .Any(listofStudent =>
                                        listofStudent.Id == student.Id);


               return storageStudent switch
               {
                   true => await this.studentValidatorService.ModifyStudentAsync(student),

                   _ => await this.studentValidatorService.AddStudentAsync(student)
               };
           });
        public async ValueTask<Student> TryRemoveStudentAsync(Guid studentId) =>
            await TryCatch(async () =>
            {
                ValidateOnTryRemove(studentId);

                return await
                    this.studentValidatorService
                        .RemoveStudentAsync(studentId);
            });
    }
}
