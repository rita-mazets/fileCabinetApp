using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class to get date from configuration.
    /// </summary>
    public class ValidateParam
    {
        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int FirstNameMin { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int FirstNameMax { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int LastNameMin { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int LastNameMax { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public DateTime DateOfBirthFrom { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public DateTime DateOfBirthTo { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int HeightMin { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int HeightMax { get; set; }

        /// <summary>
        /// Gets or sets initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="ValidateParam"/> class.
        /// </value>
        public int SalaryMin { get; set; }
    }
}
