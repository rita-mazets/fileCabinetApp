using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Reades from xml.
    /// </summary>
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
        /// Reades all records from file.
        /// </summary>
        /// <returns>All records from file.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            this.reader.Seek(0, SeekOrigin.Begin);
            XmlSerializer formatter = new XmlSerializer(typeof(Records));
            Records records = (Records)formatter.Deserialize(this.reader);
            return (IList<FileCabinetRecord>)records.RecordList;
        }
    }
}
