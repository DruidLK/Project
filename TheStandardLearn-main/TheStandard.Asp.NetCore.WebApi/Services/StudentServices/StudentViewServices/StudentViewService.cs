using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.DateTimes;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.UserManagment;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices
{
    public partial class StudentViewService : IStudentViewService
    {
        private readonly IStudentProcessingService studentProcessingService;
        private readonly IUserManagmentBroker userManagmentBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentViewService(
            IStudentProcessingService studentProcessingService,
            IUserManagmentBroker userManagmentBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.studentProcessingService = studentProcessingService;
            this.userManagmentBroker = userManagmentBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<StudentView> RegisterStudentAsync(StudentView studentView) =>
            await TryCatch(async () =>
            {
                Student inputStudent =
                    MapToStudent(studentView);

                Student storageStudent =
                    await this.studentProcessingService
                        .UpsertStudentAsync(inputStudent);

                StudentView retrievedStudent =
                     MapToStudentView(storageStudent);

                return retrievedStudent;
            });
        public IQueryable<StudentView> RetrieveStudents() =>
            TryCatch(() =>
            {
                IQueryable<Student> storageStudents =
                      this.studentProcessingService
                              .RetrieveStudentsAsync();

                var students = MapListToStudentView(storageStudents);

                return students;
            });
        public async ValueTask<StudentView> RetrieveStudentByIdAsync(Guid studentId) =>
            await TryCatch(async () =>
            {
                Student storageStudent =
                     await this.studentProcessingService
                         .RetrieveMatchingStudentByIdAsync(studentId);

                return MapToStudentView(storageStudent);
            });
        public async ValueTask<StudentView> ModifyStudentAsync(StudentView studentView) =>
            await TryCatch(async () =>
            {
                Student storageStudent =
                     await this.studentProcessingService
                         .RetrieveMatchingStudentByIdAsync(studentView.Id);

                Student updateStudent =
                      UpdateStudent(studentView, storageStudent);

                Student maybeStorage =
                   await this.studentProcessingService
                     .UpsertStudentAsync(updateStudent);

                return MapToStudentView(maybeStorage);
            });
        public async ValueTask<StudentView> RemoveStudentAsync(Guid studentId) =>
           await TryCatch(async () =>
            {
                Student storageStudent =
                  await this.studentProcessingService
                      .TryRemoveStudentAsync(studentId);

                return MapToStudentView(storageStudent);
            });
        private Student MapToStudent(StudentView studentView)
        {
            DateTimeOffset now =
                this.dateTimeBroker.GetDateTime();

            Guid currentlyLoggedInUser =
                this.userManagmentBroker.GetCurrentlyLoggedIn();

            return new Student
            {
                Id = Guid.NewGuid(),
                UserId = studentView.UserId,
                IdentityNumber = studentView.IdentityNumber,
                FirstName = studentView.FirstName,
                MiddleName = studentView.MiddleName,
                LastName = studentView.LastName,
                Gender = (Gender)studentView.GenderView,
                BirthDate = studentView.BirthDate,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedBy = currentlyLoggedInUser,
                UpdatedBy = currentlyLoggedInUser
            };
        }
        private static StudentView MapToStudentView(Student student)
        {
            return new StudentView()
            {
                Id = student.Id,
                UserId = student.UserId,
                IdentityNumber = student.IdentityNumber,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                GenderView = (GenderView)student.Gender,
                BirthDate = student.BirthDate
            };
        }
        private static IQueryable<StudentView> MapListToStudentView(IQueryable<Student> students) =>
            students.Select(property =>
                new StudentView
                {
                    Id = property.Id,
                    UserId = property.UserId,
                    IdentityNumber = property.IdentityNumber,
                    FirstName = property.FirstName,
                    MiddleName = property.MiddleName,
                    LastName = property.LastName,
                    GenderView = (GenderView)property.Gender,
                    BirthDate = property.BirthDate
                });
        private Student UpdateStudent(StudentView studentView, Student student)
        {
            DateTimeOffset now =
               this.dateTimeBroker.GetDateTime();

            Guid currentlyLoggedInUser =
                this.userManagmentBroker.GetCurrentlyLoggedIn();

            return new()
            {
                Id = student.Id,
                UserId = studentView.UserId,
                IdentityNumber = studentView.IdentityNumber,
                FirstName = studentView.FirstName,
                MiddleName = studentView.MiddleName,
                LastName = studentView.LastName,
                Gender = (Gender)studentView.GenderView,
                BirthDate = studentView.BirthDate,
                CreatedDate = student.CreatedDate,
                UpdatedDate = now,
                CreatedBy = student.CreatedBy,
                UpdatedBy = currentlyLoggedInUser
            };
        }
    }
}
