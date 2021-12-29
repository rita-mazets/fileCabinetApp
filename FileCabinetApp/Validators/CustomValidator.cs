using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates custom parameter.
    /// </summary>
    public class CustomValidator : CompositeValidator, IRecordValidator
    {
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 100),
                new LastNameValidator(2, 100),
                new DateOfBirthValidator(new DateTime(1940, 1, 1), DateTime.Now),
                new HeightValidator(70, 250),
                new SalaryValidator(100),
                new TypeValidator(),
            })
        {
        }
    }
}
