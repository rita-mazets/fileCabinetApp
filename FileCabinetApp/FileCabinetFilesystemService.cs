using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        private static int maxNameLength = 60;
        private static int recordSize = sizeof(int) + (2 * maxNameLength) + (3 * sizeof(int)) + sizeof(short) + sizeof(decimal) + sizeof(char);

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
            fileCabinetRecord.Id = this.GetStat() + 1;
            var recordToBytes = RecordToBytes(fileCabinetRecord);
            this.fileStream.Write(recordToBytes, 0, recordToBytes.Length);

            return fileCabinetRecord.Id;
        }

        private static byte[] RecordToBytes(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var bytes = new byte[recordSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(record.Id);

                var firstnameBytes = Encoding.UTF8.GetBytes(record.FirstName.PadRight(maxNameLength));
                var lastnameBytes = Encoding.UTF8.GetBytes(record.LastName.PadRight(maxNameLength));
                var nameBuffer = new byte[maxNameLength];

                binaryWriter.Write(firstnameBytes);
                binaryWriter.Write(lastnameBytes);
                binaryWriter.Write(record.DateOfBirth.Year);
                binaryWriter.Write(record.DateOfBirth.Month);
                binaryWriter.Write(record.DateOfBirth.Day);
                binaryWriter.Write(record.Height);
                binaryWriter.Write(record.Salary);
                binaryWriter.Write(record.Type);
            }

            return bytes;
        }

        private static FileCabinetRecord BytesToFileCabinetRecord(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var record = new FileCabinetRecord();

            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                record.Id = binaryReader.ReadInt32();

                var firstnameBuffer = binaryReader.ReadBytes(maxNameLength);
                record.FirstName = Encoding.UTF8.GetString(firstnameBuffer, 0, maxNameLength).Replace(" ", string.Empty);
                var lastnameBuffer = binaryReader.ReadBytes(maxNameLength);
                record.LastName = Encoding.UTF8.GetString(lastnameBuffer, 0, maxNameLength).Replace(" ", string.Empty);

                int year = binaryReader.ReadInt32();
                int month = binaryReader.ReadInt32();
                int day = binaryReader.ReadInt32();
                record.DateOfBirth = new DateTime(year, month, day);

                record.Height = binaryReader.ReadInt16();
                record.Salary = binaryReader.ReadDecimal();
                record.Type = binaryReader.ReadChar();
            }

            return record;
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to edit data.</param>
        public void EditRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            this.fileStream.Seek(0, SeekOrigin.Begin);
            var recordBuffer = new byte[recordSize];
            int offset = 0;

            while (this.fileStream.Read(recordBuffer, 0, recordBuffer.Length) > 0)
            {
                var record = BytesToFileCabinetRecord(recordBuffer);
                if (record.Id == fileCabinetRecord.Id)
                {
                    record.FirstName = fileCabinetRecord.FirstName;
                    record.LastName = fileCabinetRecord.LastName;
                    record.DateOfBirth = fileCabinetRecord.DateOfBirth;
                    record.Height = fileCabinetRecord.Height;
                    record.Salary = fileCabinetRecord.Salary;
                    record.Type = fileCabinetRecord.Type;

                    var recordToBytes = RecordToBytes(fileCabinetRecord);
                    this.fileStream.Seek(offset * recordSize, SeekOrigin.Begin);
                    this.fileStream.Write(recordToBytes, 0, recordToBytes.Length);
                    break;
                }

                offset++;
            }
        }

        /// <summary>
        /// Searches in the dictionary for data by firstName and return array where FirstName is equal firstName .
        /// </summary>
        /// <param name="firstName">Param to search.</param>
        /// <returns>Array where FirstName is equal firstName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> list = this.ReturnRecordList();
            List<FileCabinetRecord> result = list.Where(item => item.FirstName == firstName).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>
        /// Searches in the dictionary for data by lastName and return array where LastName is equal lastName .
        /// </summary>
        /// <param name="lastName">Param to search.</param>
        /// <returns>Array where LastName is equal lastName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> list = this.ReturnRecordList();
            List<FileCabinetRecord> result = list.Where(item => item.LastName == lastName).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>
        /// Searches in the dictionary for data by dateOfBirth and return array where DateOfBirth is equal dateOfBirth .
        /// </summary>
        /// <param name="dateOfBirth">Param to search.</param>
        /// <returns>Array where DateOfBirth is equal dateOfBirth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> list = this.ReturnRecordList();
            List<FileCabinetRecord> result = list.Where(item => item.DateOfBirth == dateOfBirth).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> list = this.ReturnRecordList();

            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        private List<FileCabinetRecord> ReturnRecordList()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            List<FileCabinetRecord> list = new ();
            var recordBuffer = new byte[recordSize];

            while (this.fileStream.Read(recordBuffer, 0, recordBuffer.Length) > 0)
            {
                var record = BytesToFileCabinetRecord(recordBuffer);
                list.Add(record);
            }

            return list;
        }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Record count.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / recordSize);
        }

        /// <summary>
        /// Creates snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var recordsInFile = this.GetRecords();
            foreach (var record in snapshot.Records)
            {
                try
                {
                    this.recordValidator.ValidateParameters(record);

                    if (!recordsInFile.Where(item => item.Id == record.Id).Any())
                    {
                        this.CreateRecord(record);
                    }
                    else
                    {
                        this.EditRecord(record);
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Error!{e.Message}. Record:{record.Id}");
                }
            }

            return snapshot.Records;
        }
    }
}
