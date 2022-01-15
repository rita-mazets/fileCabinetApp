using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Printer interface.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="records">Parameter to print.</param>
        /// <returns>string.</returns>
        public string Print(IEnumerable<FileCabinetRecord> records);
    }
}
