using System.Collections.Generic;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Describes record and delete status.
    /// </summary>
    public class FileRecord : FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets status of record.
        /// </summary>
        /// <value>
        /// Describes status of record.
        /// </value>
        public short IsDeleted { get; set; }
    }
}