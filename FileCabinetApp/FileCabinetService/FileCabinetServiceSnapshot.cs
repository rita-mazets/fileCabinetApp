using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Gets initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.list);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">Parameter to Initialize list.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
        }

        /// <summary>
        /// Saves date to csv file.
        /// </summary>
        /// <param name="streamWriter">Stream to write.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            csvWriter.Write(this.list);
        }

        /// <summary>
        /// Saves date to xml file.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void SaveToXml(XmlWriter writer)
        {
            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            xmlWriter.Write(this.list);
        }

        /// <summary>
        /// Loads date from csv file.
        /// </summary>
        /// <param name="stramReader">Stream to read.</param>
        public void LoadFromCsvFile(StreamReader stramReader)
        {
            var reader = new FileCabinetRecordCsvReader(stramReader);
            this.list = (List<FileCabinetRecord>)reader.ReadAll();
        }

        /// <summary>
        /// Loads date from xml file.
        /// </summary>
        /// <param name="srteam">Stream to read.</param>
        public void LoadFromXmlFile(FileStream srteam)
        {
            var reader = new FileCabinetRecordXmlReader(srteam);
            this.list = (List<FileCabinetRecord>)reader.ReadAll();
        }
    }
}
