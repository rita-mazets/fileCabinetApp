using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

            bool isPlus = false;

            if (fileCabinetRecord.Id == 0)
            {
                fileCabinetRecord.Id = this.GetStat().Item1 + 1;
                isPlus = true;
            }

            if (isPlus)
            {
                while (this.FindId(fileCabinetRecord.Id).Count > 0)
                {
                    fileCabinetRecord.Id++;
                }
            }
            else if (this.FindId(fileCabinetRecord.Id).Count > 0 && !isPlus)
            {
                throw new ArgumentException("Record with this id was create");
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
        private void EditRecord(FileCabinetRecord fileCabinetRecord)
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
                        var rec = (FileRecord)fileCabinetRecord;
                        if (rec.IsDeleted == 0)
                        {
                            recordToBytes = RecordToBytes(fileCabinetRecord);
                        }
                        else
                        {
                            recordToBytes = RecordToBytes(record, 1);
                            removeRecords.Add(record.Id);
                        }
                    }
                    else
                    {
                        recordToBytes = RecordToBytes(fileCabinetRecord);
                    }

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

            foreach (var item in result)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        public ReadOnlyCollection<FileRecord> FindByIsDeleted()
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.IsDeleted == 1).ToList();

            return new ReadOnlyCollection<FileRecord>(result);
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

            foreach (var item in result)
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

            foreach (var item in result)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        private List<FileRecord> FindId(int id)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.Id == id && item.IsDeleted != 1).ToList();

            return result;
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
        public (int, int) GetStat()
        {
            return ((int)(this.fileStream.Length / recordSize), this.FindByIsDeleted().Count);
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

        private void Remove(int id)
        {
            var record = new FileRecord { Id = id, IsDeleted = 1 };
            this.EditRecord(record);
        }

        public int Purge()
        {
            var count = this.WriteInNewFile();
            this.fileStream.Close();
            string pathDB = "cabinet-records.db";
            var fileDB = new FileInfo(pathDB);
            var fileNew = new FileInfo("newFile");
            fileDB.Delete();
            fileNew.MoveTo(pathDB);
            this.fileStream = new FileStream(pathDB, FileMode.OpenOrCreate);

            return count;
        }

        private int WriteInNewFile()
        {
            int count = 0;
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var recordBuffer = new byte[recordSize];

            using (var streamWriter = new FileStream("newFile", FileMode.OpenOrCreate))
            {
                while (this.fileStream.Read(recordBuffer, 0, recordBuffer.Length) > 0)
                {
                    var record = BytesToFileCabinetRecord(recordBuffer);
                    if (record.IsDeleted == 0)
                    {
                        count++;
                        byte[] recordToBytes;
                        recordToBytes = RecordToBytes(record);
                        streamWriter.Write(recordToBytes, 0, recordToBytes.Length);
                    }
                }
            }

            return count;
        }

        public void Delete(string name, string value)
        {
            switch (name)
            {
                case "id":
                    this.Remove(Convert.ToInt32(value, CultureInfo.CurrentCulture));
                    break;
                case "firstname":
                    this.DeleteItem(this.FindByFirstName(value));
                    break;
                case "lastname":
                    this.DeleteItem(this.FindByLastName(value));
                    break;
                case "dateofbirth":
                    this.DeleteItem(this.FindDateOfBirth(Convert.ToDateTime(value, CultureInfo.CurrentCulture)));
                    break;
                case "height":
                    this.DeleteItem(this.FindByHeight(Convert.ToInt16(value, CultureInfo.CurrentCulture)));
                    break;
                case "salary":
                    this.DeleteItem(this.FindBySalary(Convert.ToDecimal(value, CultureInfo.CurrentCulture)));
                    break;
                case "type":
                    this.DeleteItem(this.FindByType(value[0]));
                    break;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByType(char type)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.Type == type && item.IsDeleted != 1).ToList();

            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in result)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindBySalary(decimal salary)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.Salary == salary && item.IsDeleted != 1).ToList();

            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in result)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByHeight(short height)
        {
            var list = this.ReturnRecordList();
            var result = list.Where(item => item.Height == height && item.IsDeleted != 1).ToList();

            List<FileCabinetRecord> cabinetList = new();

            foreach (var item in result)
            {
                if (item.IsDeleted != 1)
                {
                    cabinetList.Add((FileCabinetRecord)item);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(cabinetList);
        }

        private void DeleteItem(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records is null)
            {
                Console.WriteLine("Records with this parameter not found");
            }
            else
            {
                var text = string.Empty;
                foreach (var record in records)
                {
                    this.Remove(record.Id);
                    text += $"#{record.Id}";
                }

                Console.WriteLine($"Records with id {text} was deleted.");
            }
        }

        public void Update(string parameters)
        {
            var param = parameters.ToLower(CultureInfo.CurrentCulture).Split("where");

            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentException("Parameters not write.");
            }

            if (param.Length == 1)
            {
                foreach (var item in this.GetRecords().ToList())
                {
                    this.WriteParamAfterSet(param[0], item);
                }
            }

            if (param.Length == 2)
            {
                var afterWhere = this.WriteParamAfterWhere(param[1]);

                foreach (var item in afterWhere)
                {
                    this.WriteParamAfterSet(param[0], item);
                }
            }
        }

        private void WriteParamAfterSet(string param, FileCabinetRecord record)
        {
            var values = param.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!values[0].StartsWith("set", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("Data is not correct. Input \"set\"");
            }
            else
            {
                values[0] = values[0].Replace("set ", string.Empty);
            }


            for (int i = 0; i < values.Length - 1; i += 2)
            {
                values[i] = values[i].Trim(' ');
                switch (values[i])
                {
                    case "firstname":
                        record.FirstName = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' }) : throw new ArgumentException("incorrect syntax after firstname");
                        break;
                    case "lastname":
                        record.LastName = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' }) : throw new ArgumentException("incorrect syntax after lastname");
                        break;
                    case "dateofbirth":
                        record.DateOfBirth = values[i + 1].Contains('\'') ? Convert.ToDateTime(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after dateOfBirth");
                        break;
                    case "heigth":
                        record.Height = values[i + 1].Contains('\'') ? Convert.ToInt16(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after height");
                        break;
                    case "salary":
                        record.Salary = values[i + 1].Contains('\'') ? Convert.ToDecimal(values[i + 1].Trim(new char[] { '\'', ' ' }), CultureInfo.CurrentCulture) : throw new ArgumentException("incorrect syntax after salary");
                        break;
                    case "type":
                        record.Type = values[i + 1].Contains('\'') ? values[i + 1].Trim(new char[] { '\'', ' ' })[0] : throw new ArgumentException("incorrect syntax after type");
                        break;
                    default:
                        throw new ArgumentException("incorrect parametr after set");
                }
            }

            this.EditRecord(record);
        }

        private ReadOnlyCollection<FileCabinetRecord> WriteParamAfterWhere(string param)
        {
            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            string[] keyValues;
            if (param.Contains("or"))
            {
                List<FileCabinetRecord> result = new();
                keyValues = param.Split("or", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < keyValues.Length; i++)
                {
                    result.AddRange(FindRecordsByParameters(this.GetRecords(), keyValues[i]));
                }

                records = new ReadOnlyCollection<FileCabinetRecord>(result);
            }
            else
            {
                keyValues = param.Split("and", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < keyValues.Length; i++)
                {
                    if (i == 0)
                    {
                        records = FindRecordsByParameters(this.GetRecords(), keyValues[i]);
                    }
                    else
                    {
                        records = FindRecordsByParameters(records, keyValues[i]);
                    }
                }
            }

            if (keyValues is null)
            {
                return null;
            }

            return records;
        }


        private static ReadOnlyCollection<FileCabinetRecord> FindRecordsByParameters(IEnumerable<FileCabinetRecord> records, string keyValue)
        {
            List<FileCabinetRecord> result = new();
            var param = keyValue.Split("=");
            param[0] = param[0].Trim(' ');
            param[1] = param[1].Trim(' ');
            switch (param[0])
            {
                case "id":
                    result = records.Where(i => i.Id == Convert.ToInt32(param[1].Trim('\''), CultureInfo.CurrentCulture)).ToList();
                    break;
                case "firstname":
                    result = records.Where(i => i.FirstName == param[1].Trim('\'')).ToList();
                    break;
                case "lastname":
                    result = records.Where(i => i.LastName == param[1].Trim('\'')).ToList();
                    break;
                case "dateofbirth":
                    result = records.Where(i => i.DateOfBirth == Convert.ToDateTime(param[1].Trim('\''), CultureInfo.CurrentCulture)).ToList();
                    break;
                case "height":
                    result = records.Where(i => i.Height == Convert.ToInt16(param[1].Trim('\''), CultureInfo.CurrentCulture)).ToList();
                    break;
                case "salary":
                    result = records.Where(i => i.Salary == Convert.ToDecimal(param[1].Trim('\''), CultureInfo.CurrentCulture)).ToList();
                    break;
                case "type":
                    result = records.Where(i => i.Type == param[1].Trim('\'')[0]).ToList();
                    break;
                default:
                    throw new ArgumentException("Incorrect parametr after where");
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public void Select(string parameters)
        {
            var param = parameters.ToLower(CultureInfo.CurrentCulture).Split("where");

            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentException("Parameters not write.");
            }

            var nameString = this.WriteParamAfterSelect(param[0]);

            if (param.Length == 1)
            {
                this.PrintTable(nameString.ToArray(), this.GetRecords());
            }

            if (param.Length == 2)
            {
                var afterWhere = this.WriteParamAfterWhere(param[1]);
                this.PrintTable(nameString.ToArray(), afterWhere);
            }
        }

        private void PrintTable(string[] namesString, IEnumerable<FileCabinetRecord> records)
        {
            var table = new ConsoleTable(namesString);

            foreach (var record in records)
            {
                List<object> values = new List<object>();
                foreach (var item in namesString)
                {
                    switch (item.ToLower(CultureInfo.CurrentCulture))
                    {
                        case "id":
                            values.Add(record.Id);
                            break;
                        case "firstname":
                            values.Add(record.FirstName);
                            break;
                        case "lastname":
                            values.Add(record.LastName);
                            break;
                        case "dateofbirth":
                            values.Add(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture));
                            break;
                        case "heigth":
                            values.Add(record.Height);
                            break;
                        case "salary":
                            values.Add(record.Salary);
                            break;
                        case "type":
                            values.Add(record.Type);
                            break;
                    }
                }

                table.AddRow(values.ToArray());
            }

            table.Configure(o => o.NumberAlignment = Alignment.Right).Write(Format.Alternative);
        }

        private List<string> WriteParamAfterSelect(string param)
        {
            var values = param.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<string> nameString = new();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim(' ');
                switch (values[i])
                {
                    case "id":
                        nameString.Add("Id");
                        break;
                    case "firstname":
                        nameString.Add("FirstName");
                        break;
                    case "lastname":
                        nameString.Add("LastName");
                        break;
                    case "dateofbirth":
                        nameString.Add("DateOfBirth");
                        break;
                    case "heigth":
                        nameString.Add("Heigth");
                        break;
                    case "salary":
                        nameString.Add("Salary");
                        break;
                    case "type":
                        nameString.Add("Type");
                        break;
                    default:
                        throw new ArgumentException("incorrect parametr after select");
                }
            }

            return nameString;
        }
    }
}
