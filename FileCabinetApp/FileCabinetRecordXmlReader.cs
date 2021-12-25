using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private FileStream reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Parameter to initialize reader.</param>
        public FileCabinetRecordXmlReader(FileStream reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Write records to file.
        /// </summary>
        public IList<FileCabinetRecord> ReadAll()
        {
            this.reader.Seek(0, SeekOrigin.Begin);
            XmlSerializer formatter = new XmlSerializer(typeof(Records));
            Records records = (Records)formatter.Deserialize(this.reader);
            return (IList<FileCabinetRecord>)records.RecordList;


        }

        private static FileCabinetRecord PareString(string line)
        {
            FileCabinetRecord record = new FileCabinetRecord();

            var items = line.Split(",");

            if (!int.TryParse(items[0], out int id))
            {
                throw new ArgumentException("Id in file is not int", nameof(line));
            }

            record.Id = id;
            record.FirstName = items[1];
            record.LastName = items[2];

            if (!DateTime.TryParse(items[3], out DateTime date))
            {
                throw new ArgumentException("Id in file is not int", nameof(line));
            }

            if (!short.TryParse(items[4], out short height))
            {
                throw new ArgumentException("Id in file is not int", nameof(line));
            }

            if (!decimal.TryParse(string.Concat(items[5], ",", items[6]), out decimal salary))
            {
                throw new ArgumentException("Id in file is not int", nameof(line));
            }

            record.DateOfBirth = date;
            record.Height = height;
            record.Salary = salary;
            record.Type = items[7][0];

            return record;
        }
    }
}
