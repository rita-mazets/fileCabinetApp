using System;
using System.Globalization;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates custom parameter.
    /// </summary>
    public class CustomValidator : CompositeValidator, IRecordValidator
    {
        public CustomValidator(ValidateParam param)
            : base(new IRecordValidator[]
            {
               new FirstNameValidator(param.FirstNameMin, param.FirstNameMax),
               new LastNameValidator(param.LastNameMin, param.LastNameMax),
               new DateOfBirthValidator(param.DateOfBirthFrom, param.DateOfBirthTo),
               new HeightValidator(param.HeightMin, param.HeightMax),
               new SalaryValidator(param.SalaryMin),
               new TypeValidator(),
            })
        {
        }
    }
}
