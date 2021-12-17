using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with default records.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates default validator.
        /// </summary>
        /// <returns>Created validator.</returns>
        public static IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
