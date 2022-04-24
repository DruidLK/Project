using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices.ModelViews.ViewExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentViewService studentViewService;

        public StudentsController(IStudentViewService studentViewService) =>
            this.studentViewService = studentViewService;

        [HttpPost(Name = "PostStudent")]
        public async ValueTask<ActionResult<StudentView>> Post([FromBody] StudentView studentView)
        {
            try
            {
                StudentView storageStudent =
                    await this.studentViewService
                        .RegisterStudentAsync(studentView);

                return CreatedAtAction(
                    nameof(GetId),
                        new
                        {
                            studentId = storageStudent.Id,
                            Controller = "Students"
                        },
                                     storageStudent);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
            {
                IDictionary innerErrors =
                    GetInnerErrors(studentViewDependencyValidationException);


                return BadRequest(error: innerErrors);
            }
            catch (StudentViewDependencyException studentViewDependencyException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyException);

                return Problem(detail: innerMessage);
            }
            catch (StudentViewServiceException studentViewServiceException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewServiceException);

                return Problem(detail: innerMessage);
            }
        }

        [HttpGet(Name = "GetStudents")]
        public ActionResult<IQueryable<StudentView>> Get()
        {
            try
            {
                IQueryable<StudentView> storageStudents =
                    this.studentViewService
                           .RetrieveStudents();

                return Ok(storageStudents);
            }
            catch (StudentViewDependencyException studentViewDependencyException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyException);

                return Problem(detail: innerMessage);
            }
            catch (StudentViewServiceException studentViewServiceException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewServiceException);

                return Problem(detail: innerMessage);
            }
        }

        [HttpGet("{studentId:Guid}", Name = "GetStudentById")]
        public async ValueTask<ActionResult<StudentView>> GetId([FromRoute] Guid studentId)
        {
            try
            {
                StudentView storageStudent =
                    await this.studentViewService
                        .RetrieveStudentByIdAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
                when (studentViewDependencyValidationException.InnerException is StudentNotFoundException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyValidationException);

                return NotFound(value: innerMessage);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
            {
                IDictionary innerErrors =
                    GetInnerErrors(studentViewDependencyValidationException);

                return BadRequest(error: innerErrors);
            }
            catch (StudentViewDependencyException studentViewDependencyException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyException);

                return Problem(detail: innerMessage);
            }
            catch (StudentViewServiceException studentViewServiceException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewServiceException);

                return Problem(detail: innerMessage);
            }
        }

        [HttpPut(Name = "PutStudent")]
        public async ValueTask<ActionResult<StudentView>> Put([FromBody] StudentView studentView)
        {
            try
            {
                StudentView storageStudent =
                    await this.studentViewService
                        .ModifyStudentAsync(studentView);

                return Ok(storageStudent);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
                when (studentViewDependencyValidationException.InnerException is StudentNotFoundException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyValidationException);

                return NotFound(value: innerMessage);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
            {
                IDictionary innerErrors =
                    GetInnerErrors(studentViewDependencyValidationException);

                return BadRequest(error: innerErrors);
            }
            catch (StudentViewDependencyException studentViewDependencyException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyException);

                return Problem(detail: innerMessage);
            }
            catch (StudentViewServiceException studentViewServiceException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewServiceException);

                return Problem(detail: innerMessage);
            }
        }

        [HttpDelete("{studentId:Guid}", Name = "DeleteStudentById")]
        public async ValueTask<ActionResult<StudentView>> Delete([FromRoute] Guid studentId)
        {
            try
            {
                StudentView storageStudent =
                    await this.studentViewService
                        .RemoveStudentAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidation)
                when (studentViewDependencyValidation.InnerException is StudentNotFoundException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyValidation);

                return NotFound(value: innerMessage);
            }
            catch (StudentViewDependencyValidationException studentViewDependencyValidationException)
            {
                IDictionary innerErrors =
                    GetInnerErrors(studentViewDependencyValidationException);

                return BadRequest(error: innerErrors);
            }
            catch (StudentViewDependencyException studentViewDependencyException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewDependencyException);

                return Problem(detail: innerMessage);
            }
            catch (StudentViewServiceException studentViewServiceException)
            {
                string innerMessage =
                    GetInnerMessage(studentViewServiceException);

                return Problem(detail: innerMessage);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        private static IDictionary GetInnerErrors(Exception exception) =>
            exception.InnerException.Data;
    }
}
