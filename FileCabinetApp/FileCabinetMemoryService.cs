using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

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

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
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

        /// <summary>
        /// Searches in the dictionary for data by firstName and return array where FirstName is equal firstName .
        /// </summary>
        /// <param name="firstName">Param to search.</param>
        /// <returns>Array where FirstName is equal firstName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("incorrect firstName", nameof(firstName));
            }

            firstName = firstName.ToLower(CultureInfo.CurrentCulture);

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                throw new ArgumentException("firstName don't consist in dictionary", nameof(firstName));
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
        }

        /// <summary>
        /// Searches in the dictionary for data by lastName and return array where LastName is equal lastName .
        /// </summary>
        /// <param name="lastName">Param to search.</param>
        /// <returns>Array where LastName is equal lastName.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("incorrect lastName", nameof(lastName));
            }

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                throw new ArgumentException("lastName don't consist in dictionary", nameof(lastName));
            }

            lastName = lastName.ToLower(CultureInfo.CurrentCulture);
            return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
        }

        /// <summary>
        /// Searches in the dictionary for data by dateOfBirth and return array where DateOfBirth is equal dateOfBirth .
        /// </summary>
        /// <param name="dateOfBirth">Param to search.</param>
        /// <returns>Array where DateOfBirth is equal dateOfBirth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("dateOfBirth don't consist in dictionary", nameof(dateOfBirth));
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]);
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

        public int Purge()
        {
            return -1;
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

        private ReadOnlyCollection<FileCabinetRecord> FindByType(char type)
        {
            var result = this.list.Where(item => item.Type == type).ToList();

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        private ReadOnlyCollection<FileCabinetRecord> FindBySalary(decimal salary)
        {
            var result = this.list.Where(item => item.Salary == salary).ToList();

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByHeight(short height)
        {
            var result = this.list.Where(item => item.Height == height).ToList();

            return new ReadOnlyCollection<FileCabinetRecord>(result);
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
                foreach (var item in this.list)
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
            Dictionary<string, string> dict = new();
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

        private List<FileCabinetRecord> WriteParamAfterWhere(string param)
        {
            List<FileCabinetRecord> records = new ();
            var keyValues = param.Split("and", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keyValues.Length; i++)
            {
                if (i == 0)
                {
                    records = FindRecordsByParameters(this.list, keyValues[i]);
                }
                else
                {
                    records = FindRecordsByParameters(records, keyValues[i]);
                }
            }

            if (keyValues.Length == 0)
            {
                return null;
            }

            return records;
        }


        private static List<FileCabinetRecord> FindRecordsByParameters(List<FileCabinetRecord> records, string keyValue)
        {
            List<FileCabinetRecord> result = new ();
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

            return result;
        }
    }
}
