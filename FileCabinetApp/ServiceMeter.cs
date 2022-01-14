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

        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.Restore(snapshot);
            stopWatch.Stop();
            Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        public void Select(string parameters)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            this.service.Select(parameters);
            stopWatch.Stop();
            Console.WriteLine($"Update method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        public void Update(string parameters)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            this.service.Update(parameters);
            stopWatch.Stop();
            Console.WriteLine($"Update method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }
    }
}
