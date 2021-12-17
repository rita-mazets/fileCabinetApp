using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Describes the records.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Describes the records.
        /// </summary>
        /// <value>
        /// Idemtity number.
        /// </value>
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Height { get; set; }

        public decimal Salary { get; set; }

        public char Type { get; set; }
    }
}
