using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class DataConverter
    {
        public static (bool, string, string) StringConverter(string str)
        {
            return (true, str, str);
        }

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
