using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates lastname.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLenght">Parameter to initialize minLenght parameter.</param>
        /// <param name="maxLenght">Parameter to initialize maxLenght parameter.</param>
        public LastNameValidator(int minLenght, int maxLenght)
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

            if (string.IsNullOrWhiteSpace(value.LastName) || value.LastName.Length < 2 || value.LastName.Length > 60)
            {
                throw new ArgumentException("incorrect lastName", nameof(value));
            }
        }
    }
}
