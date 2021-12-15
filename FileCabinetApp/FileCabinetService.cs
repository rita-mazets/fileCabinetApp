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
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short heigth, decimal salary, char type)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("incorrect firstName", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("incorrect lastName", nameof(lastName));
            }

            if (heigth < 0 || heigth > 250)
            {
                throw new ArgumentException("incorrect height", nameof(heigth));
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("incorrect dateOfBirth", nameof(dateOfBirth));
            }

            if (salary < 0)
            {
                throw new ArgumentException("incorrect salary", nameof(salary));
            }

            if (!char.IsLetter(type))
            {
                throw new ArgumentException("incorrect type", nameof(type));
            }

            var listItem = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Height = heigth, Salary = salary, Type = type,
            };

            this.InsertIntoCollection(listItem, firstName, lastName, dateOfBirth);
            return listItem.Id;
        }

        private void InsertIntoCollection(FileCabinetRecord item, string firstName, string lastName, DateTime date)
        {
            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstName].Add(item);

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
            }

            this.lastNameDictionary[lastName].Add(item);

            if (!this.dateOfBirthDictionary.ContainsKey(date))
            {
                this.dateOfBirthDictionary.Add(date, new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[date].Add(item);

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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short heigth, decimal salary, char type)
        {
            bool isExist = false;
            /*for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == id)
                {
                    this.list[i].FirstName = firstName;
                    this.list[i].LastName = lastName;
                    this.list[i].DateOfBirth = dateOfBirth;
                    this.list[i].Type = type;
                    this.list[i].Salary = salary;
                    this.list[i].Height = heigth;
                    isExist = true;
                    break;
                }
            }*/

            foreach (var value in this.firstNameDictionary.Values)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].Id == id)
                    {
                        if (value[i].FirstName == firstName)
                        {
                            this.firstNameDictionary[firstName][i].LastName = lastName;
                            this.firstNameDictionary[firstName][i].DateOfBirth = dateOfBirth;
                            this.firstNameDictionary[firstName][i].Salary = salary;
                            this.firstNameDictionary[firstName][i].Type = type;
                            this.firstNameDictionary[firstName][i].Height = heigth;
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

                            this.firstNameDictionary[firstName].Add(new FileCabinetRecord { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Height = heigth, Salary = salary, Type = type, });
                            return;

                        }
                    
                    }
                }
            }

            var t = this.firstNameDictionary.Values;

            if (!isExist)
            {
                throw new ArgumentException("id record is not found", nameof(id));
            }
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

            lastName = lastName.ToLower(CultureInfo.CurrentCulture);
            return this.lastNameDictionary[lastName].ToArray();
        }

        public FileCabinetRecord[] FindDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary[dateOfBirth].ToArray();
        }
    }
}
