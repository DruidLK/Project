using System;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices
{
    public partial class StudentProcessingService
    {
        private static void ValidateOnEnsure(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }

            Validate(
                (Rule: IsInvalidId(student.Id), Parameter: nameof(Student.Id)));
        }
        private static void ValidateOnUpsert(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }

            Validate(
                (Rule: IsInvalidId(student.Id), Parameter: nameof(Student.Id)));
        }
        private static void ValidateOnRetrieveStudentById(Guid studentid)
        {
            Validate(
                (Rule: IsInvalidId(studentid), Parameter: nameof(Student.Id)));
        }
        private static void ValidateOnTryRemove(Guid studentId)
        {
            Validate(
                (Rule: IsInvalidId(studentId), Parameter: nameof(Student.Id)));
        }
        private static dynamic IsInvalidId(Guid studentId) => new
        {
            Condition = studentId == Guid.Empty,
            Message = "Id is Required."
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] Validations)
        {
            var invalidStudentException =
                new InvalidStudentException();

            foreach ((dynamic rule, string parameter) in Validations)
            {
                if (rule.Condition)
                {
                    invalidStudentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidStudentException.ThrowIfContainsErrors();
        }
    }
}
