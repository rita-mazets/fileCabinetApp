using System;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLenght;
        private int maxLenght;

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
