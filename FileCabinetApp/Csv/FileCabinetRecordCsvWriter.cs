using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes list ti csv file class.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Parameter to initialize writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write records to file.
        /// </summary>
        /// <param name="records">Parameter to write to file.</param>
        public void Write(List<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                this.writer.WriteLine(ToCsvString(record));
            }
        }

        private static string ToCsvString(FileCabinetRecord record)
        {
            return record.Id + "," + record.FirstName + "," + record.LastName + "," + record.DateOfBirth + "," + record.Height + "," + record.Salary + "," + record.Type;
        }
    }
}
