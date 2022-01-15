using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates date.
    /// </summary>
    public static class DataValidator
    {
        /// <summary>
        /// Validates name.
        /// </summary>
        /// <param name="value">Parameter to check date.</param>
        /// <returns>Returns tuple(isValidate, value converted to string).</returns>
        public static (bool, string) NameValidator(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 60)
            {
                return (false, value);
            }

            return (true, value);
        }

        /// <summary>
        /// Validates date.
        /// </summary>
        /// <param name="value">Parameter to check date.</param>
        /// <returns>Returns tuple(isValidate, value converted to string).</returns>
        public static (bool, string) DateValidator(DateTime value)
        {
            if (value < new DateTime(1950, 1, 1) || value > DateTime.Now)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Validates height.
        /// </summary>
        /// <param name="value">Parameter to check date.</param>
        /// <returns>Returns tuple(isValidate, value converted to string).</returns>
        public static (bool, string) HeightValidator(short value)
        {
            if (value < 0 || value > 250)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Validates salary.
        /// </summary>
        /// <param name="value">Parameter to check date.</param>
        /// <returns>Returns tuple(isValidate, value converted to string).</returns>
        public static (bool, string) SalaryValidator(decimal value)
        {
            if (value < 0)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Validates type.
        /// </summary>
        /// <param name="value">Parameter to check date.</param>
        /// <returns>Returns tuple(isValidate, value converted to string).</returns>
        public static (bool, string) TypeValidator(char value)
        {
            if (!char.IsLetter(value))
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }
    }
}
