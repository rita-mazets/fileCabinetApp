using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with custom record.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creates custom validator.
        /// </summary>
        /// <returns>Created validator.</returns>
        public override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
