using System;
using System.Linq;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;

namespace TheStandard.Asp.NetCore.WebApi.Brokers
{
    public partial interface IStorageBroker
    {
        ValueTask<Student> InsertStudentAsync(Student student);
        IQueryable<Student> SelectStudents();
        ValueTask<Student> SelectStudentById(Guid studentId);
        ValueTask<Student> UpdateStudentAsync(Student student);
        ValueTask<Student> DeleteStudentAsync(Student student);
    }
}
