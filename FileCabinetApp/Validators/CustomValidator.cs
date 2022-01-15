using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates custom parameter.
    /// </summary>
    public class CustomValidator : CompositeValidator, IRecordValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        /// <param name="param">Parameter to validate date.</param>
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
