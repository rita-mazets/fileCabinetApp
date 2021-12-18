using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates default parameter.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates parameter.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to validate data.</param>
        public void ValidateParameters(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentException("fileCabinetRecord is null", nameof(fileCabinetRecord));
            }

            if (string.IsNullOrWhiteSpace(fileCabinetRecord.FirstName) || fileCabinetRecord.FirstName.Length < 2 || fileCabinetRecord.FirstName.Length > 60)
            {
                throw new ArgumentException("incorrect firstName", nameof(fileCabinetRecord));
            }

            if (string.IsNullOrWhiteSpace(fileCabinetRecord.LastName) || fileCabinetRecord.LastName.Length < 2 || fileCabinetRecord.LastName.Length > 60)
            {
                throw new ArgumentException("incorrect lastName", nameof(fileCabinetRecord));
            }

            if (fileCabinetRecord.Height < 0 || fileCabinetRecord.Height > 250)
            {
                throw new ArgumentException("incorrect height", nameof(fileCabinetRecord));
            }

            if (fileCabinetRecord.DateOfBirth < new DateTime(1950, 1, 1) || fileCabinetRecord.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("incorrect dateOfBirth", nameof(fileCabinetRecord));
            }

            if (fileCabinetRecord.Salary < 0)
            {
                throw new ArgumentException("incorrect salary", nameof(fileCabinetRecord));
            }

            if (!char.IsLetter(fileCabinetRecord.Type))
            {
                throw new ArgumentException("incorrect type", nameof(fileCabinetRecord));
            }
        }
    }
}
