using System.Collections.Generic;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Create records.
    /// </summary>
    [XmlRoot("records")]
    public class Records
    {
        /// <summary>
        /// Gets initializes a new instance of the <see cref="Records"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="Records"/> class.
        /// </value>
        [XmlArray("record")]
        public List<FileCabinetRecord> RecordList = new ();
    }
}
