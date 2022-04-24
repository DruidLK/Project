using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices
{
    public partial interface IStudentValidatorService
    {
        ValueTask<Student> AddStudentAsync(Student student);
        IQueryable<Student> RetrieveAllStudents();
        ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
        ValueTask<Student> ModifyStudentAsync(Student student);
        ValueTask<Student> RemoveStudentAsync(Guid studentId);
    }
}
