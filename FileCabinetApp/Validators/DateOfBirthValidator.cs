using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates DateOfBirth.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">Parameter to initialize from paramer.</param>
        /// <param name="to">Parameter to initialize to paramer.</param>
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
