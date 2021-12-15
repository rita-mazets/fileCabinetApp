using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            Validation(fileCabinetRecord);

            var listItem = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = fileCabinetRecord.FirstName, LastName = fileCabinetRecord.LastName, DateOfBirth = fileCabinetRecord.DateOfBirth, Height = fileCabinetRecord.Height, Salary = fileCabinetRecord.Salary, Type = fileCabinetRecord.Type,
            };

            this.InsertIntoCollection(listItem, fileCabinetRecord.FirstName, fileCabinetRecord.LastName, fileCabinetRecord.DateOfBirth);
            return listItem.Id;
        }

        private static void Validation(FileCabinetRecord fileCabinetRecord)
        {
            if (string.IsNullOrWhiteSpace(fileCabinetRecord.FirstName) || fileCabinetRecord.FirstName.Length < 2 || fileCabinetRecord.FirstName.Length > 60)
            {
                throw new ArgumentException("incorrect firstName", nameof(fileCabinetRecord.FirstName));
            }

            if (string.IsNullOrWhiteSpace(fileCabinetRecord.LastName) || fileCabinetRecord.LastName.Length < 2 || fileCabinetRecord.LastName.Length > 60)
            {
                throw new ArgumentException("incorrect lastName", nameof(fileCabinetRecord.LastName));
            }

            if (fileCabinetRecord.Height < 0 || fileCabinetRecord.Height > 250)
            {
                throw new ArgumentException("incorrect height", nameof(fileCabinetRecord.Height));
            }

            if (fileCabinetRecord.DateOfBirth < new DateTime(1950, 1, 1) || fileCabinetRecord.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("incorrect dateOfBirth", nameof(fileCabinetRecord.DateOfBirth));
            }

            if (fileCabinetRecord.Salary < 0)
            {
                throw new ArgumentException("incorrect salary", nameof(fileCabinetRecord.Salary));
            }

            if (!char.IsLetter(fileCabinetRecord.Type))
            {
                throw new ArgumentException("incorrect type", nameof(fileCabinetRecord.Type));
            }
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

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            Validation(fileCabinetRecord);

            bool isExist = false;
            isExist = this.EditListItem(fileCabinetRecord, isExist);
            this.EditDictionaryItem(fileCabinetRecord);

            if (!isExist)
            {
                throw new ArgumentException("id record is not found", nameof(fileCabinetRecord.Id));
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

        public FileCabinetRecord[] FindByFirstName(string firstName)
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

            return this.firstNameDictionary[firstName].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
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
            return this.lastNameDictionary[lastName].ToArray();
        }

        public FileCabinetRecord[] FindDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("dateOfBirth don't consist in dictionary", nameof(dateOfBirth));
            }

            return this.dateOfBirthDictionary[dateOfBirth].ToArray();
        }
    }
}
