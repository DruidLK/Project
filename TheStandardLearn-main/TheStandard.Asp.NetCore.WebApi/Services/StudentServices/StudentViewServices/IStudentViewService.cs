using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices
{
    public interface IStudentViewService
    {
        ValueTask<StudentView> RegisterStudentAsync(StudentView studentView);
        ValueTask<StudentView> ModifyStudentAsync(StudentView studentView);
        ValueTask<StudentView> RemoveStudentAsync(Guid studentId);
        ValueTask<StudentView> RetrieveStudentByIdAsync(Guid studentId);
        IQueryable<StudentView> RetrieveStudents();
    }
}
