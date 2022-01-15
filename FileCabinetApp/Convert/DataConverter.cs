using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with convert.
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Converts string to string.
        /// </summary>
        /// <param name="str">Parameter to convert string to string.</param>
        /// <returns>Record (possible to convert, string, convert to string value).</returns>
        public static (bool, string, string) StringConverter(string str)
        {
            return (true, str, str);
        }

        /// <summary>
        /// Converts string to DateTime.
        /// </summary>
        /// <param name="str">Parameter to convert string to DateTime.</param>
        /// <returns>Record (possible to convert, string, convert to DateTime value).</returns>
        public static (bool, string, DateTime) DateConverter(string str)
        {
            DateTime value;
            if (DateTime.TryParse(str, out value))
            {
                return (true, str, value);
            }
            else
            {
                return (false, str, DateTime.MinValue);
            }
        }

        /// <summary>
        /// Converts string to Short.
        /// </summary>
        /// <param name="str">Parameter to convert string to short.</param>
        /// <returns>Record (possible to convert, string, convert to short value).</returns>
        public static (bool, string, short) ShortConverter(string str)
        {
            short value;
            if (short.TryParse(str, out value))
            {
                return (true, str, value);
            }
            else
            {
                return (false, str, short.MinValue);
            }
        }

        /// <summary>
        /// Converts string to decimal.
        /// </summary>
        /// <param name="str">Parameter to convert string to decimal.</param>
        /// <returns>Record (possible to convert, string, convert to decimal value).</returns>
        public static (bool, string, decimal) DecimalConverter(string str)
        {
            decimal value;
            if (decimal.TryParse(str, out value))
            {
                return (true, str, value);
            }
            else
            {
                return (false, str, decimal.MinValue);
            }
        }

        /// <summary>
        /// Converts string to char.
        /// </summary>
        /// <param name="str">Parameter to convert string to char.</param>
        /// <returns>Record (possible to convert, string,  convert to char value).</returns>
        public static (bool, string, char) CharConverter(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (false, str, char.MinValue);
            }
            else
            {
                return (true, str, str[0]);
            }
        }
    }
}
