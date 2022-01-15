using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Converts string where are words "set", "select", "where".
    /// </summary>
    public static class ConvertFunctionString
    {
        /// <summary>
        /// Converts parameters after select to List.
        /// </summary>
        /// <param name="param">Parameter to convert data.</param>
        /// <returns>List of string of name for heder.</returns>
        public static List<string> WriteParamAfterSelect(string param)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var values = param.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<string> nameString = new ();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim(' ');
                switch (values[i])
                {
                    case "id":
                        nameString.Add("id");
                        break;
                    case "firstname":
                        nameString.Add("FirstName");
                        break;
                    case "lastname":
                        nameString.Add("LastName");
                        break;
                    case "dateofbirth":
                        nameString.Add("DateOfBirth");
                        break;
                    case "heigth":
                        nameString.Add("Heigth");
                        break;
                    case "salary":
                        nameString.Add("Salary");
                        break;
                    case "type":
                        nameString.Add("Type");
                        break;
                }
            }

            return nameString;
        }

        /// <summary>
        /// Converts parameters after Set to List.
        /// </summary>
        /// <param name="param">Parameter to convert data.</param>
        /// <param name="record">Parameter to set data.</param>
        /// <param name="action">Parameter to action with date.</param>
        public static void WriteParamAfterSet(string param, FileCabinetRecord record, Action<FileCabinetRecord> action = null)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var values = param.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!values[0].StartsWith("set", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("Data is not correct. Input \"set\"");
            }
            else
            {
                values[0] = values[0].Replace("set ", string.Empty);
            }

            for (int i = 0; i < values.Length - 1; i += 2)
            {
                values[i] = values[i].Trim(' ');
                switch (values[i])
                {
                    case "firstname":
                        record.FirstName = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' }) : throw new ArgumentException("incorrect syntax after firstname");
                        break;
                    case "lastname":
                        record.LastName = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' }) : throw new ArgumentException("incorrect syntax after lastname");
                        break;
                    case "dateofbirth":
                        record.DateOfBirth = values[i + 1].Contains('\'') ? Convert.ToDateTime(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after dateOfBirth");
                        break;
                    case "heigth":
                        record.Height = values[i + 1].Contains('\'') ? Convert.ToInt16(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after height");
                        break;
                    case "salary":
                        record.Salary = values[i + 1].Contains('\'') ? Convert.ToDecimal(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after salary");
                        break;
                    case "type":
                        record.Type = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' })[0] : throw new ArgumentException("incorrect syntax after type");
                        break;
                    default:
                        throw new ArgumentException("incorrect parametr after set");
                }
            }

            if (action is not null)
            {
                action(record);
            }
        }

        /// <summary>
        /// Converts parameters after where to List.
        /// </summary>
        /// <param name="param">Parameter to convert data.</param>
        /// <param name="func">Parameter to action with data.</param>
        /// <param name="list">Parameter to search date.</param>
        /// <returns>List with records to manipulation.</returns>
        public static ReadOnlyCollection<FileCabinetRecord> WriteParamAfterWhere(string param, Func<IEnumerable<FileCabinetRecord>, string, ReadOnlyCollection<FileCabinetRecord>> func, IEnumerable<FileCabinetRecord> list)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            string[] keyValues;
            if (param.Contains("or"))
            {
                List<FileCabinetRecord> result = new ();
                keyValues = param.Split("or", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < keyValues.Length; i++)
                {
                    result.AddRange(func(list, keyValues[i]));
                }

                records = new ReadOnlyCollection<FileCabinetRecord>(result);
            }
            else
            {
                keyValues = param.Split("and", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < keyValues.Length; i++)
                {
                    if (i == 0)
                    {
                        records = func(list, keyValues[i]);
                    }
                    else
                    {
                        records = func(records, keyValues[i]);
                    }
                }
            }

            if (keyValues is null)
            {
                return null;
            }

            return records;
        }
    }
}
