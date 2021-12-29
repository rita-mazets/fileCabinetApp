using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates default parameter.
    /// </summary>
    public class DefaultValidator : CompositeValidator, IRecordValidator
    {
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now),
                new HeightValidator(40, 250),
                new SalaryValidator(20),
                new TypeValidator(),
            })
        {
        }
    }
}
