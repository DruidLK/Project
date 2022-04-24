using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheStandard.Asp.NetCore.Tests.Acceptance.Models.Students;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.Brokers.ApiBrokers
{
    public partial class ApiBrokerTest
    {
        private const string studentUrl = "Api/Students";

        public async ValueTask<Student> PostStudent(Student student) =>
           await this.apiFactoryClient
                .PostContentAsync(relativeUrl: studentUrl, content: student);

        public async ValueTask<List<Student>> GetStudents() =>
            await this.apiFactoryClient
                .GetContentAsync<List<Student>>(relativeUrl: studentUrl);

        public async ValueTask<Student> GetStudent(Guid studentId) =>
            await this.apiFactoryClient
                .GetContentAsync<Student>(relativeUrl: $"{studentUrl}/{studentId}");

        public async ValueTask<Student> PutStudent(Student student) =>
            await this.apiFactoryClient
                .PostContentAsync(relativeUrl: studentUrl, content: student);

        public async ValueTask<Student> DeleteStudent(Guid studentId) =>
            await this.apiFactoryClient
                .DeleteContentAsync<Student>(relativeUrl: $"{studentUrl}/{studentId}");
    }
}
