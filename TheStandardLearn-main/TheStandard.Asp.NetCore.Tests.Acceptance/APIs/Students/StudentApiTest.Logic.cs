//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Force.DeepCloner;
//using RESTFulSense.Exceptions;
//using TheStandard.Asp.NetCore.Tests.Acceptance.Models.Students;
//using Xunit;

//namespace TheStandard.Asp.NetCore.Tests.Acceptance.APIs.Students
//{
//    Un-Comment this to test The Api End-Points.
//    public partial class StudentApiTest
//    {
//        [Fact]
//        public async Task ShouldAddStudentOnPostWhenPassedIn()
//        {
//            // Arrange - Given
//            Student randomStudent = CreateRandomStudent();
//            Student inputStudent = randomStudent;
//            Student expectedStudent = inputStudent;

//            // Act - When
//            Student actualStudent =
//                await this.apiBrokerTest
//                    .PostStudent(inputStudent);

//            // Assert - Then
//            actualStudent.Should().BeEquivalentTo(expectedStudent,
//                config =>
//                    config.Excluding(property => property.Id));

//            await this.apiBrokerTest.DeleteStudent(actualStudent.Id);
//        }

//        [Fact]
//        public async Task ShouldRetrieveStudentsOnGetWhenCalled()
//        {
//            // Arrange - Given
//            IEnumerable<Student> randomStudents =
//                GetRandomStudentsList();

//            IEnumerable<Student> inputStudent = randomStudents;
//            IEnumerable<Student> expectedStudents = inputStudent;

//            await PostStudents(inputStudent);
//            await Task.Delay(500);
//            await PostStudents(inputStudent);
//            await Task.Delay(500);

//            // Act - When
//            IEnumerable<Student> actualStudents =
//                await this.apiBrokerTest
//                    .GetStudents();

//            // Assert - Then
//            actualStudents.Should().BeEquivalentTo(expectedStudents,
//                config =>
//                    config.Excluding(property => property.Id));

//            await DeleteStudents(actualStudents);
//        }

//        [Fact]
//        public async Task ShouldRetrieveStudentByIdOnGetByIdWhenCalled()
//        {
//            // Arrange - Given

//            Student randomStudent = CreateRandomStudent();
//            Student inputStudent = randomStudent;
//            Student expectedStudent = inputStudent;

//            Student studentPost =
//                await this.apiBrokerTest
//                  .PostStudent(inputStudent);

//            // Act - When
//            Student actualStudent =
//                await this.apiBrokerTest
//                    .GetStudent(studentPost.Id);

//            // Assert - Then
//            actualStudent.Should().BeEquivalentTo(expectedStudent,
//                config =>
//                    config.Excluding(property => property.Id));

//            await this.apiBrokerTest.DeleteStudent(actualStudent.Id);
//        }

//        [Fact]
//        public async Task ShouldModifyStudentOnPutWhenPassedIn()
//        {
//            // Arrange - Given
//            Student randomStudent = CreateRandomStudent();
//            Student inputStudent = randomStudent;
//            Student storageStudent = inputStudent.DeepClone();
//            inputStudent.FirstName = "fadi";

//            await this.apiBrokerTest
//                .PostStudent(storageStudent);

//            Student expectedStudent = inputStudent;

//            // Act - When
//            Student actualStudent =
//                await this.apiBrokerTest
//                    .PutStudent(inputStudent);

//            // Assert - Then
//            actualStudent.Should().BeEquivalentTo(expectedStudent,
//                config =>
//                    config.Excluding(property => property.Id));

//            await this.apiBrokerTest.DeleteStudent(actualStudent.Id);
//        }

//        [Fact]
//        public async Task ShouldRemoveStudentOnDeleteWhenPassedIn()
//        {
//            // Arrange - Given
//            Student randomStudent =
//                await this.apiBrokerTest
//                    .PostStudent(CreateRandomStudent());

//            Student inputStudent = randomStudent;
//            Student expectedStudent = inputStudent;

//            // Act - When
//            Student actualStudent =
//                await this.apiBrokerTest
//                    .DeleteStudent(inputStudent.Id);

//            ValueTask<Student> getStudentByIdTask =
//              this.apiBrokerTest
//                .GetStudent(actualStudent.Id);

//            // Assert - Then
//            inputStudent.Should().BeEquivalentTo(expectedStudent);

//            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
//               getStudentByIdTask.AsTask());
//        }
//    }
//}