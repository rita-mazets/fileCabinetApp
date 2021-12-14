using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            var listItem = new FileCabinetRecord { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };
            this.list.Add(listItem);
            return 0;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            // TODO: добавьте реализацию метода
            return 0;
        }
    }
}
