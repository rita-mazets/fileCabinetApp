using System.Collections.Generic;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with xml records.
    /// </summary>
    [XmlRoot("records")]
    public class Records
    {
        /// <summary>
        /// Gets or sets xmlrecords.
        /// </summary>
        /// <value>
        /// Describes the xml records.
        /// </value>
        [XmlArray("record")]
        public List<FileRecord> RecordList = new ();
    }
}
