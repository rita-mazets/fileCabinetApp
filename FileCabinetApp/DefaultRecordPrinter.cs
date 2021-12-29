using System;
using System.Collections.Generic;
using FileCabinetApp.interfaces;

namespace FileCabinetApp
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
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
    }
}
