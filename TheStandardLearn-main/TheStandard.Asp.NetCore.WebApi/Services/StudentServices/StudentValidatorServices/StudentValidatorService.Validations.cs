using System;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models;
using TheStandard.Asp.NetCore.WebApi.Brokers.Models.StudentExceptions;

namespace TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices
{
    public partial class StudentValidatorService
    {
        private static void ValidateOnAdd(Student student)
        {
            IsStudentNull(student);

            Validate(
                  (Rule: IsInvalidId(student.Id), Parameter: nameof(Student.Id)),
                  (Rule: IsInvalidString(student.UserId), Parameter: nameof(Student.UserId)),
                  (Rule: IsInvalidString(student.IdentityNumber), Parameter: nameof(Student.IdentityNumber)),
                  (Rule: IsInvalidString(student.FirstName), Parameter: nameof(Student.FirstName)),
                  (Rule: IsInvalidString(student.MiddleName), Parameter: nameof(Student.MiddleName)),
                  (Rule: IsInvalidString(student.LastName), Parameter: nameof(Student.LastName)),
                  (Rule: IsInvalidDate(student.BirthDate), Parameter: nameof(Student.BirthDate)),
                  (Rule: IsInvalidDate(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                  (Rule: IsInvalidDate(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                  (Rule: IsInvalidId(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
                  (Rule: IsInvalidId(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)),
                  (Rule: IsNotSameDate(student.CreatedDate, student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)));

        }
        private static void ValidateOnRetrieve(Guid studentId)
        {
            Validate((Rule: IsInvalidId(studentId), Parameter: nameof(Student.Id)));
        }
        private static void ValidateOnModify(Student student)
        {
            IsStudentNull(student);

            Validate(
                (Rule: IsInvalidId(student.Id), Parameter: nameof(Student.Id)),
                (Rule: IsInvalidString(student.UserId), Parameter: nameof(Student.UserId)),
                (Rule: IsInvalidString(student.IdentityNumber), Parameter: nameof(Student.IdentityNumber)),
                (Rule: IsInvalidString(student.FirstName), Parameter: nameof(Student.FirstName)),
                (Rule: IsInvalidString(student.MiddleName), Parameter: nameof(Student.MiddleName)),
                (Rule: IsInvalidString(student.LastName), Parameter: nameof(Student.LastName)),
                (Rule: IsInvalidDate(student.BirthDate), Parameter: nameof(Student.BirthDate)),
                (Rule: IsInvalidDate(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                (Rule: IsInvalidDate(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsInvalidId(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
                (Rule: IsInvalidId(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)));
        }
        private static void ValidateOnRemove(Guid studentId)
        {
            Validate((Rule: IsInvalidId(studentId), Parameter: nameof(Student.Id)));
        }
        private static void IsStudentNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }
        private static void ValidateStorageOnRetrieve(Student storageStudent, Guid studentId)
        {
            if (storageStudent is null)
            {
                throw new StudentNotFoundException(studentId);
            }
        }
        private static void ValidateStorageOnModify(Student storageStudent, Student student)
        {
            if (storageStudent is null)
            {
                throw new StudentNotFoundException(student.Id);
            }

            Validate(
                (Rule: IsNotSameDate(storageStudent.CreatedDate, student.CreatedDate), Parameter: nameof(Student.CreatedDate)));
        }
        private static void ValidateStorageOnRemove(Student storageStudent, Guid studentId)
        {
            if (storageStudent is null)
            {
                throw new StudentNotFoundException(studentId);
            }
        }
        private static dynamic IsInvalidId(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is Required."
        };
        private static dynamic IsInvalidString(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is Required."
        };
        private static dynamic IsNotSameDate(DateTimeOffset created, DateTimeOffset updated) => new
        {
            Condition = created != updated,
            Message = "The Dates Should Be Equal."
        };
        private static dynamic IsInvalidDate(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is Required."
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] Validations)
        {
            InvalidStudentException invalidStudentException = new();

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
