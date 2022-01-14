using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ConsoleTables;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with memoryrecords.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        private Dictionary<string, ReadOnlyCollection<FileCabinetRecord>> firstnameOneCashe = new ();
        private Dictionary<string, ReadOnlyCollection<FileCabinetRecord>> lastnameOneCashe = new ();
        private Dictionary<DateTime, ReadOnlyCollection<FileCabinetRecord>> dateofbirthOneCashe = new ();
        private Dictionary<short, ReadOnlyCollection<FileCabinetRecord>> heightOneCashe = new ();
        private Dictionary<decimal, ReadOnlyCollection<FileCabinetRecord>> salaryOneCashe = new ();
        private Dictionary<char, ReadOnlyCollection<FileCabinetRecord>> typeOneCashe = new ();

        private Dictionary<(IEnumerable<FileCabinetRecord>, string), ReadOnlyCollection<FileCabinetRecord>> firstnameTwoCashe = new ();
        private Dictionary<(IEnumerable<FileCabinetRecord>, string), ReadOnlyCollection<FileCabinetRecord>> lastnameTwoCashe = new ();
        private Dictionary<(IEnumerable<FileCabinetRecord>, DateTime), ReadOnlyCollection<FileCabinetRecord>> dateofbirthTwoCashe = new ();
        private Dictionary<(IEnumerable<FileCabinetRecord>, short), ReadOnlyCollection<FileCabinetRecord>> heightTwoCashe = new ();
        private Dictionary<(IEnumerable<FileCabinetRecord>, decimal), ReadOnlyCollection<FileCabinetRecord>> salaryTwoCashe = new ();
        private Dictionary<(IEnumerable<FileCabinetRecord>, char), ReadOnlyCollection<FileCabinetRecord>> typeTwoCashe = new ();
#pragma warning disable SA1305 // Field names should not use Hungarian notation
        private Dictionary<(IEnumerable<FileCabinetRecord>, int), ReadOnlyCollection<FileCabinetRecord>> idTwoCashe = new ();
#pragma warning restore SA1305 // Field names should not use Hungarian notation

        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="recordValidator">Parameter to choose validator.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
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

            this.ClearCashe();

            this.recordValidator.ValidateParameters(fileCabinetRecord);

            var listItem = new FileCabinetRecord
            {
                Id = fileCabinetRecord.Id == 0 ? this.list.Count + 1 : fileCabinetRecord.Id,
                FirstName = fileCabinetRecord.FirstName, LastName = fileCabinetRecord.LastName, DateOfBirth = fileCabinetRecord.DateOfBirth, Height = fileCabinetRecord.Height, Salary = fileCabinetRecord.Salary, Type = fileCabinetRecord.Type,
            };

            this.InsertIntoCollection(listItem, fileCabinetRecord.FirstName, fileCabinetRecord.LastName, fileCabinetRecord.DateOfBirth);
            return listItem.Id;
        }

        private static void InsertIntoDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dict, T key, FileCabinetRecord item)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<FileCabinetRecord>());
            }

            dict[key].Add(item);
        }

        private void InsertIntoCollection(FileCabinetRecord item, string firstName, string lastName, DateTime date)
        {
            InsertIntoDictionary<string>(this.firstNameDictionary, firstName, item);
            InsertIntoDictionary<string>(this.lastNameDictionary, lastName, item);
            InsertIntoDictionary<DateTime>(this.dateOfBirthDictionary, date, item);
            this.list.Add(item);
        }

        private ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Record count.</returns>
        public (int, int) GetStat()
        {
            return (this.list.Count, 0);
        }

        private void EditRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            this.recordValidator.ValidateParameters(fileCabinetRecord);

            bool isExist = false;
            isExist = this.EditListItem(fileCabinetRecord, isExist);
            this.EditDictionaryItem(fileCabinetRecord);

            if (!isExist)
            {
                throw new ArgumentException("id record is not found", nameof(fileCabinetRecord));
            }
        }

        private void EditDictionaryItem(FileCabinetRecord fileCabinetRecord)
        {
            var firstName = fileCabinetRecord.FirstName;
            foreach (var value in this.firstNameDictionary.Values)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].Id == fileCabinetRecord.Id)
                    {
                        if (value[i].FirstName == fileCabinetRecord.FirstName)
                        {
                            this.firstNameDictionary[firstName][i].LastName = fileCabinetRecord.LastName;
                            this.firstNameDictionary[firstName][i].DateOfBirth = fileCabinetRecord.DateOfBirth;
                            this.firstNameDictionary[firstName][i].Salary = fileCabinetRecord.Salary;
                            this.firstNameDictionary[firstName][i].Type = fileCabinetRecord.Type;
                            this.firstNameDictionary[firstName][i].Height = fileCabinetRecord.Height;
                            return;
                        }
                        else
                        {
                            string oldName = value[i].FirstName;
                            this.firstNameDictionary[oldName].RemoveAt(i);

                            if (this.firstNameDictionary[oldName].Count == 0)
                            {
                                this.firstNameDictionary.Remove(oldName);
                            }

                            if (!this.firstNameDictionary.ContainsKey(firstName))
                            {
                                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
                            }

                            this.firstNameDictionary[firstName].Add(new FileCabinetRecord { Id = fileCabinetRecord.Id, FirstName = firstName, LastName = fileCabinetRecord.LastName, DateOfBirth = fileCabinetRecord.DateOfBirth, Height = fileCabinetRecord.Height, Salary = fileCabinetRecord.Salary, Type = fileCabinetRecord.Type, });
                            return;
                        }
                    }
                }
            }
        }

        private bool EditListItem(FileCabinetRecord fileCabinetRecord, bool isExist)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == fileCabinetRecord.Id)
                {
                    this.list[i].FirstName = fileCabinetRecord.FirstName;
                    this.list[i].LastName = fileCabinetRecord.LastName;
                    this.list[i].DateOfBirth = fileCabinetRecord.DateOfBirth;
                    this.list[i].Type = fileCabinetRecord.Type;
                    this.list[i].Salary = fileCabinetRecord.Salary;
                    this.list[i].Height = fileCabinetRecord.Height;
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstnameOneCashe.ContainsKey(firstName))
            {
                return this.firstnameOneCashe[firstName];
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("incorrect firstName", nameof(firstName));
            }

            firstName = firstName.ToLower(CultureInfo.CurrentCulture);

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                throw new ArgumentException("firstName don't consist in dictionary", nameof(firstName));
            }

            var result = new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            this.firstnameOneCashe.Add(firstName, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByFirstName(IEnumerable<FileCabinetRecord> list, string firstName)
        {
            if (this.firstnameTwoCashe.ContainsKey((list, firstName)))
            {
                return this.firstnameTwoCashe[(list, firstName)];
            }

            var result = list.Where(item => item.FirstName == firstName).ToList();

            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.firstnameTwoCashe.Add((list, firstName), records);
            return records;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("incorrect lastName", nameof(lastName));
            }

            if (this.lastnameOneCashe.ContainsKey(lastName))
            {
                return this.lastnameOneCashe[lastName];
            }

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                throw new ArgumentException("lastName don't consist in dictionary", nameof(lastName));
            }

            lastName = lastName.ToLower(CultureInfo.CurrentCulture);
            var result = new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            this.lastnameOneCashe.Add(lastName, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByLastName(IEnumerable<FileCabinetRecord> list, string lastName)
        {
            if (this.lastnameTwoCashe.ContainsKey((list, lastName)))
            {
                return this.lastnameTwoCashe[(list, lastName)];
            }

            var result = list.Where(item => item.LastName == lastName).ToList();

            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.lastnameTwoCashe.Add((list, lastName), records);
            return records;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("dateOfBirth don't consist in dictionary", nameof(dateOfBirth));
            }

            if (this.dateofbirthOneCashe.ContainsKey(dateOfBirth))
            {
                return this.dateofbirthOneCashe[dateOfBirth];
            }

            var result = new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]);
            this.dateofbirthOneCashe.Add(dateOfBirth, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(IEnumerable<FileCabinetRecord> list, DateTime dateOfBirth)
        {
            if (this.dateofbirthTwoCashe.ContainsKey((list, dateOfBirth)))
            {
                return this.dateofbirthTwoCashe[(list, dateOfBirth)];
            }

            var result = list.Where(item => item.DateOfBirth == dateOfBirth).ToList();

            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.dateofbirthTwoCashe.Add((list, dateOfBirth), records);
            return records;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByType(char type)
        {
            if (this.typeOneCashe.ContainsKey(type))
            {
                return this.typeOneCashe[type];
            }

            var records = this.list.Where(item => item.Type == type).ToList();

            var result = new ReadOnlyCollection<FileCabinetRecord>(records);
            this.typeOneCashe.Add(type, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByType(IEnumerable<FileCabinetRecord> list, char type)
        {
            if (this.typeTwoCashe.ContainsKey((list, type)))
            {
                return this.typeTwoCashe[(list, type)];
            }

            var records = list.Where(item => item.Type == type).ToList();

            var result = new ReadOnlyCollection<FileCabinetRecord>(records);
            this.typeTwoCashe.Add((list, type), result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindBySalary(decimal salary)
        {
            if (this.salaryOneCashe.ContainsKey(salary))
            {
                return this.salaryOneCashe[salary];
            }

            var records = this.list.Where(item => item.Salary == salary).ToList();

            var result = new ReadOnlyCollection<FileCabinetRecord>(records);
            this.salaryOneCashe.Add(salary, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindBySalary(IEnumerable<FileCabinetRecord> list, decimal salary)
        {
            if (this.salaryTwoCashe.ContainsKey((list, salary)))
            {
                return this.salaryTwoCashe[(list, salary)];
            }

            var result = list.Where(item => item.Salary == salary).ToList();

            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.salaryTwoCashe.Add((list, salary), records);
            return records;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByHeight(short height)
        {
            if (this.heightOneCashe.ContainsKey(height))
            {
                return this.heightOneCashe[height];
            }

            var records = this.list.Where(item => item.Height == height).ToList();

            var result = new ReadOnlyCollection<FileCabinetRecord>(records);
            this.heightOneCashe.Add(height, result);
            return result;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByHeight(IEnumerable<FileCabinetRecord> list, short height)
        {
            if (this.heightTwoCashe.ContainsKey((list, height)))
            {
                return this.heightTwoCashe[(list, height)];
            }

            var result = list.Where(item => item.Height == height).ToList();
            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.heightTwoCashe.Add((list, height), records);
            return records;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindById(IEnumerable<FileCabinetRecord> list, int id)
        {
            if (this.idTwoCashe.ContainsKey((list, id)))
            {
                return this.idTwoCashe[(list, id)];
            }

            var result = list.Where(item => item.Id == id).ToList();
            var records = new ReadOnlyCollection<FileCabinetRecord>(result);
            this.idTwoCashe.Add((list, id), records);
            return records;
        }

        /// <summary>
        /// Creates snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list);
        }

        /// <summary>
        /// Creates snapshot.
        /// </summary>
        /// <param name="snapshot">Parameter to restore data.</param>
        /// <returns>Snapshot.</returns>
        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var records = snapshot.Records;
            foreach (var record in snapshot.Records)
            {
                try
                {
                    this.recordValidator.ValidateParameters(record);
                    if (!this.list.Where(item => item.Id == record.Id).Any())
                    {
                        this.InsertIntoCollection(record, record.FirstName, record.LastName, record.DateOfBirth);
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
            if (!this.list.Where(item => item.Id == id).Any())
            {
                throw new ArgumentException($"Record {id} doesn't exist.");
            }

            var record = this.list.Where(item => item.Id == id).First();

            this.list.Remove(record);
            RemoveDictionary<string>(this.firstNameDictionary, record, record.FirstName);
            RemoveDictionary<string>(this.lastNameDictionary, record, record.LastName);
            RemoveDictionary<DateTime>(this.dateOfBirthDictionary, record, record.DateOfBirth);
        }

        /// <summary>
        /// Returns -1.
        /// </summary>
        /// <returns>-1.</returns>
        public int Purge()
        {
            return -1;
        }

        /// <summary>
        /// Deletes record.
        /// </summary>
        /// <param name="name">Parameter to find parameter to delete record.</param>
        /// /// <param name="value">Parameter to delete record.</param>
        public void Delete(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.ClearCashe();

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

        /// <summary>
        /// Updates record.
        /// </summary>
        /// <param name="parameters">Parameter update record.</param>
        public void Update(string parameters)
        {
            var param = parameters?.ToLower(CultureInfo.CurrentCulture).Split("where");

            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentException("Parameters not write.");
            }

            this.ClearCashe();

            if (param.Length == 1)
            {
                foreach (var item in this.list)
                {
                    WriteParamAfterSet(param[0], item);
                }
            }

            if (param.Length == 2)
            {
                var afterWhere = this.WriteParamAfterWhere(param[1]);

                foreach (var item in afterWhere)
                {
                    WriteParamAfterSet(param[0], item);
                }
            }
        }

        private void ClearCashe()
        {
            this.firstnameOneCashe.Clear();
            this.firstnameTwoCashe.Clear();
            this.lastnameOneCashe.Clear();
            this.lastnameTwoCashe.Clear();
            this.dateofbirthOneCashe.Clear();
            this.dateofbirthTwoCashe.Clear();
            this.heightOneCashe.Clear();
            this.heightTwoCashe.Clear();
            this.salaryOneCashe.Clear();
            this.salaryTwoCashe.Clear();
            this.typeOneCashe.Clear();
            this.typeTwoCashe.Clear();
        }

        private ReadOnlyCollection<FileCabinetRecord> WriteParamAfterWhere(string param)
        {
            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            string[] keyValues;
            if (param.Contains("or"))
            {
                List<FileCabinetRecord> result = new ();
                keyValues = param.Split("or", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < keyValues.Length; i++)
                {
                     result.AddRange(this.FindRecordsByParameters(this.list, keyValues[i]));
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
                        records = this.FindRecordsByParameters(this.list, keyValues[i]);
                    }
                    else
                    {
                        records = this.FindRecordsByParameters(records, keyValues[i]);
                    }
                }
            }

            if (keyValues is null)
            {
                return null;
            }

            return records;
        }

        /// <summary>
        /// Selects record.
        /// </summary>
        /// <param name="parameters">Parameter select record.</param>
        public void Select(string parameters)
        {
            var param = parameters?.ToLower(CultureInfo.CurrentCulture).Split("where");

            if (string.IsNullOrEmpty(parameters))
            {
                PrintTable(new string[] { "id", "firstname", "lastname", "dateofbirth", "heigth", "salary", "type" }, this.GetRecords());
            }

            var nameString = WriteParamAfterSelect(param[0]);

            if (param.Length == 1)
            {
                PrintTable(nameString.ToArray(), this.list);
            }

            if (param.Length == 2)
            {
                var afterWhere = this.WriteParamAfterWhere(param[1]);
                PrintTable(nameString.ToArray(), afterWhere);
            }
        }

        private static void WriteParamAfterSet(string param, FileCabinetRecord record)
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
        }

        private static void PrintTable(string[] namesString, IEnumerable<FileCabinetRecord> records)
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

        private static List<string> WriteParamAfterSelect(string param)
        {
            var values = param.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<string> nameString = new ();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim(' ');
                switch (values[i])
                {
                    case "id":
                        nameString.Add("id");
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
                }
            }

            return nameString;
        }

        private static void RemoveDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, FileCabinetRecord record, T param)
        {
            if (dictionary[param].Count > 1)
            {
                dictionary[param].Remove(record);
            }
            else if (dictionary[param].Count == 1)
            {
                dictionary.Remove(param);
            }
        }

        private ReadOnlyCollection<FileCabinetRecord> FindRecordsByParameters(IEnumerable<FileCabinetRecord> records, string keyValue)
        {
            ReadOnlyCollection<FileCabinetRecord> result;
            var param = keyValue.Split("=");
            param[0] = param[0].Trim(' ');
            param[1] = param[1].Trim(' ');
            switch (param[0])
            {
                case "id":
                    result = this.FindById(records, Convert.ToInt32(param[1].Trim('\''), CultureInfo.CurrentCulture));
                    break;
                case "firstname":
                    result = this.FindByFirstName(records, param[1].Trim('\''));
                    break;
                case "lastname":
                    result = this.FindByLastName(records, param[1].Trim('\''));
                    break;
                case "dateofbirth":
                    result = this.FindDateOfBirth(records, Convert.ToDateTime(param[1].Trim('\''), CultureInfo.CurrentCulture));
                    break;
                case "height":
                    result = this.FindByHeight(records, Convert.ToInt16(param[1].Trim('\''), CultureInfo.CurrentCulture));
                    break;
                case "salary":
                    result = this.FindBySalary(records, Convert.ToDecimal(param[1].Trim('\''), CultureInfo.CurrentCulture));
                    break;
                case "type":
                    result = this.FindByType(records, param[1].Trim('\'')[0]);
                    break;
                default:
                    throw new ArgumentException("Incorrect parametr after where");
            }

            return result;
        }
    }
}
