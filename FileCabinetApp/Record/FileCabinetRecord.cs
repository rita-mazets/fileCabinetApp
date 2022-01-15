using System;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Describes the records.
    /// </summary>
    [Serializable]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Gets or sets describes the id.
        /// </summary>
        /// <value>
        /// Describes the id.
        /// </value>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets describes the name.
        /// </summary>
        /// <value>
        /// Describes the name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets describes the lastname.
        /// </summary>
        /// <value>
        /// Describes the lastname.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets describes the date of birth.
        /// </summary>
        /// <value>
        /// Describes the date of birth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets describes the height.
        /// </summary>
        /// <value>
        /// Describes the height.
        /// </value>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets describes the salary.
        /// </summary>
        /// <value>
        /// Describes the salary.
        /// </value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets describes the type.
        /// </summary>
        /// <value>
        /// Describes the type.
        /// </value>
        public char Type { get; set; }
    }
}
