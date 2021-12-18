using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates parameter.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates parameter.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to validate data.</param>
        public void ValidateParameters(FileCabinetRecord fileCabinetRecord);
    }
}
