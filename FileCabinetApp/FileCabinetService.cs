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
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="recordValidator">Parameter to choose validator.</param>
        public FileCabinetService(IRecordValidator recordValidator)
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
                Id = this.list.Count + 1,
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
        public int GetStat()
        {
            return this.list.Count;
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
    }
}
