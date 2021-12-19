using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes list ti xml file class.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Parameter to initialize writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
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

            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("records");
            foreach (var record in records)
            {
                this.writer.WriteStartElement("record");

                this.writer.WriteAttributeString("id", record.Id.ToString(CultureInfo.CurrentCulture));

                this.writer.WriteStartElement("name");
                this.writer.WriteAttributeString("first", record.FirstName);
                this.writer.WriteAttributeString("last", record.LastName);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("dateOfBirth");
                this.writer.WriteString(record.DateOfBirth.ToString(CultureInfo.CurrentCulture));
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("height");
                this.writer.WriteString(record.Height.ToString(CultureInfo.CurrentCulture));
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("salary");
                this.writer.WriteString(record.Salary.ToString(CultureInfo.CurrentCulture));
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("type");
                this.writer.WriteString(record.Type.ToString(CultureInfo.CurrentCulture));
                this.writer.WriteEndElement();

                this.writer.WriteEndElement();
            }

            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();
        }
    }
}
