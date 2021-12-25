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
        private static int recordSize = sizeof(short) + sizeof(int) + (2 * maxNameLength) + (3 * sizeof(int)) + sizeof(short) + sizeof(decimal) + sizeof(char);
        private static RemoveRecords removeRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Parameter to initialize fileStream.</param>
        /// /// <param name="recordValidator">Parameter to initialize recordValidator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator recordValidator)
        {
            this.fileStream = fileStream;
            this.recordValidator = recordValidator;
            removeRecords = new ();
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

            if (this.FindId(fileCabinetRecord.Id).Count > 0)
            {
                throw new ArgumentException("Record with this id was create");
            }

            if (fileCabinetRecord.Id == 0)
            {
                fileCabinetRecord.Id = this.GetStat() + 1;
            }

            var recordToBytes = RecordToBytes(fileCabinetRecord);
            this.fileStream.Write(recordToBytes, 0, recordToBytes.Length);

            return fileCabinetRecord.Id;
        }

        private static byte[] RecordToBytes(FileCabinetRecord record, short isDeleted = 0)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var bytes = new byte[recordSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(isDeleted);
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

        private static FileRecord BytesToFileCabinetRecord(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var record = new FileRecord();

            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                record.IsDeleted = binaryReader.ReadInt16();
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
                    byte[] recordToBytes;
                    if (fileCabinetRecord is FileRecord)
                    {
                        recordToBytes = RecordToBytes(record, 1);
                        
                        removeRecords.Add(record.Id);
                    }
                    else
                    {
                        recordToBytes = RecordToBytes(fileCabinetRecord);
                    }
                    
                    /*record.FirstName = fileCabinetRecord.FirstName;
                    record.LastName = fileCabinetRecord.LastName;
                    record.DateOfBirth = fileCabinetRecord.DateOfBirth;
                    record.Height = fileCabinetRecord.Height;
                    record.Salary = fileCabinetRecord.Salary;
                    record.Type = fileCabinetRecord.Type;*/

                    
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
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.FirstName == firstName && item.IsDeleted != 1).ToList();

            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in list)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        /// <summary>
        /// Searches in the dictionary for data by lastName and return array where LastName is equal lastName .
        /// </summary>
        /// <param name="lastName">Param to search.</param>
        /// <returns>Array where LastName is equal lastName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.LastName == lastName && item.IsDeleted != 1).ToList();
            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in list)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        /// <summary>
        /// Searches in the dictionary for data by dateOfBirth and return array where DateOfBirth is equal dateOfBirth .
        /// </summary>
        /// <param name="dateOfBirth">Param to search.</param>
        /// <returns>Array where DateOfBirth is equal dateOfBirth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.DateOfBirth == dateOfBirth && item.IsDeleted != 1).ToList();
            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in list)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        private ReadOnlyCollection<FileCabinetRecord> FindId(int id)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.Id == id && item.IsDeleted != 1).ToList();
            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in list)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var list = this.ReturnRecordList();

            List<FileCabinetRecord> cabinetList = new ();

            foreach (var item in list)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        private List<FileRecord> ReturnRecordList()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            List<FileRecord> list = new ();
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

        public void Remove(int id)
        {
            var record = new FileRecord { Id = id, IsDeleted = 1 };
            this.EditRecord(record);
        }
    }
}
