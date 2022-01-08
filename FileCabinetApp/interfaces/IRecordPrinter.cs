using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.interfaces
{
    public interface IRecordPrinter
    {
        public string Print(IEnumerable<FileCabinetRecord> records);
    }
}
