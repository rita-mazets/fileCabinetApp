using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class DataValidator
    {
        public static (bool, string) NameValidator(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 60)
            {
                return (false, value);
            }

            return (true, value);
        }

        public static (bool, string) DateValidator(DateTime value)
        {
            if (value < new DateTime(1950, 1, 1) || value > DateTime.Now)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

        public static (bool, string) HeightValidator(short value)
        {
            if (value < 0 || value > 250)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

        public static (bool, string) SalaryValidator(decimal value)
        {
            if (value < 0)
            {
                return (false, value.ToString(CultureInfo.CurrentCulture));
            }

            return (true, value.ToString(CultureInfo.CurrentCulture));
        }

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
