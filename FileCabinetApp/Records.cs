using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    [XmlRoot("records")]
    public class Records
    {
        [XmlArray("record")]
        public List<FileRecord> RecordList = new();

    }

    public class FileRecord : FileCabinetRecord
    {
        public short IsDeleted { get; set; }
    }
}
