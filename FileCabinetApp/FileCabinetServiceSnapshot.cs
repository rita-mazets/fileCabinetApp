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
    /// Works with snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private List<FileCabinetRecord> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">Parameter to Initialize list.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Saves date do file.
        /// </summary>
        /// <param name="streamWriter">Param to write.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            csvWriter.Write(this.list);
        }

        /// <summary>
        /// Saves date do file.
        /// </summary>
        /// <param name="writer">Param to write.</param>
        public void SaveToXml(XmlWriter writer)
        {
            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            xmlWriter.Write(this.list);
        }
    }
}
