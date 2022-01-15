using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates firstname.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLenght">Parameter to initialize minLenght parameter.</param>
        /// <param name="maxLenght">Parameter to initialize maxLenght parameter.</param>
        public FirstNameValidator(int minLenght, int maxLenght)
        {
            this.minLenght = minLenght;
            this.maxLenght = maxLenght;
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

            if (string.IsNullOrWhiteSpace(value.FirstName) || value.FirstName.Length < this.minLenght || value.FirstName.Length > this.maxLenght)
            {
                throw new ArgumentException("incorrect firstName", nameof(value));
            }
        }
    }
}
