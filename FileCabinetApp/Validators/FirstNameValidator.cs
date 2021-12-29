using System;

namespace FileCabinetApp.Validators
{
    public class FirstNameValidator : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

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
