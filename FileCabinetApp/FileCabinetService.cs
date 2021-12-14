using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
            this.list.Add(listItem);
            return listItem.Id;
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
            for (int i = 0; i < this.list.Count; i++)
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
            }

            if (!isExist)
            {
                throw new ArgumentException("id record is not found", nameof(id));
            }
        }
    }
}
