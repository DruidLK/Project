using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;

namespace TheStandard.Asp.NetCore.WebApi.Brokers
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public async ValueTask<Student> InsertStudentAsync(Student student)
        {
            EntityEntry<Student> studentEntity =
                await this.Students.AddAsync(student);

            await this.SaveChangesAsync();

            return studentEntity.Entity;
        }

        public IQueryable<Student> SelectStudents() =>
            this.Students.AsQueryable();

        public async ValueTask<Student> SelectStudentById(Guid studentId)
        {
            this.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;

            Student student =
                await this.Students.FindAsync(studentId);

            return student;
        }
        public async ValueTask<Student> UpdateStudentAsync(Student student)
        {
            EntityEntry<Student> entityEntry = this.Students.Update(student);
            await this.SaveChangesAsync();

            return entityEntry.Entity;
        }
        public async ValueTask<Student> DeleteStudentAsync(Student student)
        {
            EntityEntry<Student> studentEntity = this.Students.Remove(student);
            await this.SaveChangesAsync();

            return studentEntity.Entity;
        }
    }
}
