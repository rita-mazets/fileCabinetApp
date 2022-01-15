using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Build Validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Creates validator.
        /// </summary>
        /// <returns>validator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Creates FirstNameValidator.
        /// </summary>
        /// <param name="min">Parameter to initialize min value.</param>
        /// <param name="max">Parameter to initialize max value.</param>
        /// <returns>FirstNameValidator.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates ValidateLastName.
        /// </summary>
        /// <param name="min">Parameter to initialize min value.</param>
        /// <param name="max">Parameter to initialize max value.</param>
        /// <returns>ValidateLastName.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates ValidateDateOfBirth.
        /// </summary>
        /// <param name="from">Parameter to initialize min value.</param>
        /// <param name="to">Parameter to initialize max value.</param>
        /// <returns>ValidateDateOfBirth.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Creates ValidateHeight.
        /// </summary>
        /// <param name="min">Parameter to initialize min value.</param>
        /// <param name="max">Parameter to initialize max value.</param>
        /// <returns>ValidateHeight.</returns>
        public ValidatorBuilder ValidateHeight(int min, int max)
        {
            this.validators.Add(new HeightValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates ValidateSalary.
        /// </summary>
        /// <param name="min">Parameter to validate data.</param>
        /// <returns>ValidateSalary.</returns>
        public ValidatorBuilder ValidateSalary(int min)
        {
            this.validators.Add(new SalaryValidator(min));
            return this;
        }

        /// <summary>
        /// Creates TypeValidator.
        /// </summary>
        /// <returns>TypeValidator.</returns>
        public ValidatorBuilder ValidateType()
        {
            this.validators.Add(new TypeValidator());
            return this;
        }

        /// <summary>
        /// Creates defaultvalidator.
        /// </summary>
        /// <param name="param">Parameter to validate data.</param>
        /// <returns>DefaultValidator.</returns>
        public IRecordValidator CreateDefault(ValidateParam param)
        {
            return new DefaultValidator(param);
        }

        /// <summary>
        /// Creates customvalidatorr.
        /// </summary>
        /// <param name="param">Parameter to validate data.</param>
        /// <returns>CustomValidator.</returns>
        public IRecordValidator CreateCustom(ValidateParam param)
        {
            return new CustomValidator(param);
        }
    }
}
