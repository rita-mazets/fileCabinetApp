using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates salary.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private int min;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="min">Parameter to initialize min param.</param>
        public SalaryValidator(int min)
        {
            this.min = min;
        }

        /// <summary>
        /// Validates parameter.
        /// </summary>
        /// <param name="value">Parameter to validate data.</param>
        public void ValidateParameters(FileCabinetRecord value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Salary < this.min)
            {
                throw new ArgumentException("incorrect salary", nameof(value));
            }
        }
    }
}
