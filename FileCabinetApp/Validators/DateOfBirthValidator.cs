using System;

namespace FileCabinetApp.Validators
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
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

            if (value.DateOfBirth < this.from || value.DateOfBirth > this.to)
            {
                throw new ArgumentException("incorrect dateOfBirth", nameof(value));
            }
        }
    }
}
