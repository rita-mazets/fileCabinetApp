using System;

namespace FileCabinetApp.Validators
{
    public class SalaryValidator : IRecordValidator
    {
        private int min;

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
