using System;
using System.Collections.Generic;
using System.Globalization;
using ConsoleTables;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with print date.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Prints date.
        /// </summary>
        /// <param name="records">Parameter to print in pretty view.</param>
        /// <returns>All date converted in string.</returns>
        public string Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            string result = string.Empty;
            foreach (var record in records)
            {
                result += $"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}\n";
            }

            return result;
        }

        /// <summary>
        /// Prints date in table.
        /// </summary>
        /// <param name="namesString">Parameter to print heder.</param>
        /// <param name="records">Parameter to print in pretty view.</param>
        public static void PrintTable(string[] namesString, IEnumerable<FileCabinetRecord> records)
        {
            if (namesString is null)
            {
                throw new ArgumentNullException(nameof(namesString));
            }

            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var table = new ConsoleTable(namesString);

            foreach (var record in records)
            {
                List<object> values = new List<object>();
                foreach (var item in namesString)
                {
                    switch (item.ToLower(CultureInfo.CurrentCulture))
                    {
                        case "id":
                            values.Add(record.Id);
                            break;
                        case "firstname":
                            values.Add(record.FirstName);
                            break;
                        case "lastname":
                            values.Add(record.LastName);
                            break;
                        case "dateofbirth":
                            values.Add(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture));
                            break;
                        case "heigth":
                            values.Add(record.Height);
                            break;
                        case "salary":
                            values.Add(record.Salary);
                            break;
                        case "type":
                            values.Add(record.Type);
                            break;
                    }
                }

                table.AddRow(values.ToArray());
            }

            table.Configure(o => o.NumberAlignment = Alignment.Right).Write(Format.Alternative);
        }
    }
}
