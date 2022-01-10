using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceMeter: IFileCabinetService
    {
        private IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(FileCabinetRecord fileCabinetRecord)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.CreateRecord(fileCabinetRecord);
            stopWatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public void Delete(string name, string value)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            this.service.Delete(name, value);
            stopWatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        public void EditRecord(FileCabinetRecord fileCabinetRecord)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            this.service.EditRecord(fileCabinetRecord);
            stopWatch.Stop();
            Console.WriteLine($"EditRecord method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.FindByFirstName(firstName);
            stopWatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.FindByLastName(lastName);
            stopWatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result =  this.service.FindDateOfBirth(dateOfBirth);
            stopWatch.Stop();
            Console.WriteLine($"FindDateOfBirth method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.GetRecords();
            stopWatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public (int, int) GetStat()
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.GetStat();
            stopWatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public int Insert(FileCabinetRecord fileCabinetRecord)
        {
            throw new NotImplementedException();
        }

        public int Purge()
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.Purge();
            stopWatch.Stop();
            Console.WriteLine($"Purge method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public void Remove(int id)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            this.service.Remove(id);
            stopWatch.Stop();
            Console.WriteLine($"Remove method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = this.service.Restore(snapshot);
            stopWatch.Stop();
            Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }
    }
}
