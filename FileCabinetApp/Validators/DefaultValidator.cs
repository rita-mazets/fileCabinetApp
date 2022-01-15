using System;
using System.Globalization;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates default parameter.
    /// </summary>
    public class DefaultValidator : CompositeValidator, IRecordValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        /// <param name="param">Parameter to validate date.</param>
        public DefaultValidator(ValidateParam param)
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
