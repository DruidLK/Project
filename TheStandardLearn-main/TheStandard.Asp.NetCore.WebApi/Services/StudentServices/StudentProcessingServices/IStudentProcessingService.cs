using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices
{
    public interface IStudentProcessingService
    {
        ValueTask<Student> EnsureStudentExistAsync(Student student);
        ValueTask<Student> UpsertStudentAsync(Student student);
        IQueryable<Student> RetrieveStudentsAsync();
        ValueTask<Student> RetrieveMatchingStudentByIdAsync(Guid studentId);
        ValueTask<Student> TryRemoveStudentAsync(Guid studentId);
    }
}
