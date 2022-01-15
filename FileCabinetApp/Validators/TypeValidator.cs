using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates type.
    /// </summary>
    public class TypeValidator : IRecordValidator
    {
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

            if (!char.IsLetter(value.Type))
            {
                throw new ArgumentException("incorrect type", nameof(value));
            }
        }
    }
}
