using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with records in filesystem.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Parameter to initialize fileStream.</param>
        /// /// <param name="recordValidator">Parameter to initialize recordValidator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator recordValidator)
        {
            this.fileStream = fileStream;
            this.recordValidator = recordValidator;
        }

        /// <summary>
        /// Creates new record and return id.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to insert data.</param>
        /// <returns>Record id.</returns>
        public int CreateRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            this.recordValidator.ValidateParameters(fileCabinetRecord);

            List<byte[]> list = new ();
            fileCabinetRecord.Id = this.GetStat() + 1;
            list.Add(BitConverter.GetBytes(fileCabinetRecord.Id));
            list.Add(System.Text.Encoding.UTF8.GetBytes(fileCabinetRecord.FirstName.PadRight(60)));
            list.Add(System.Text.Encoding.UTF8.GetBytes(fileCabinetRecord.LastName.PadRight(60)));
            list.Add(BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Year));
            list.Add(BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Month));
            list.Add(BitConverter.GetBytes(fileCabinetRecord.DateOfBirth.Day));
            list.Add(BitConverter.GetBytes(fileCabinetRecord.Height));
            list.Add(BitConverter.GetBytes(decimal.ToByte(fileCabinetRecord.Salary)));
            list.Add(BitConverter.GetBytes(fileCabinetRecord.Type));

            foreach (var item in list)
            {
                this.fileStream.Write(item, 0, item.Length);
            }

            return fileCabinetRecord.Id;
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to edit data.</param>
        public void EditRecord(FileCabinetRecord fileCabinetRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches in the dictionary for data by firstName and return array where FirstName is equal firstName .
        /// </summary>
        /// <param name="firstName">Param to search.</param>
        /// <returns>Array where FirstName is equal firstName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches in the dictionary for data by lastName and return array where LastName is equal lastName .
        /// </summary>
        /// <param name="lastName">Param to search.</param>
        /// <returns>Array where LastName is equal lastName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches in the dictionary for data by dateOfBirth and return array where DateOfBirth is equal dateOfBirth .
        /// </summary>
        /// <param name="dateOfBirth">Param to search.</param>
        /// <returns>Array where DateOfBirth is equal dateOfBirth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Record count.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / 142);
        }
    }
}
