using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceLogger : IFileCabinetService
    {
        private IFileCabinetService service;
        private string fileName = "log.txt";

        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(FileCabinetRecord fileCabinetRecord)
        {
            if (fileCabinetRecord is null)
            {
                throw new ArgumentNullException(nameof(fileCabinetRecord));
            }

            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Create() with FirsrName = \"{fileCabinetRecord.FirstName}\", LastName = \"{fileCabinetRecord.LastName}\", DateOfBirth = \"{fileCabinetRecord.DateOfBirth}\", Height = \"{fileCabinetRecord.Height}\", Salary = \"{fileCabinetRecord.Salary}\", Type = \"{fileCabinetRecord.Type}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.CreateRecord(fileCabinetRecord);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Create() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return -1;
                }
            }
        }

        public void Delete(string name, string value)
        {

            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Delete() with {name} = \'{value}\' ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    this.service.Delete(name, value);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Delete() was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                }
            }
        }

        
        public (int, int) GetStat()
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling GetStat(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.GetStat();
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} GetStat() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"GetStat method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return (-1, -1);
                }
            }
        }

        public int Insert(FileCabinetRecord fileCabinetRecord)
        {
            throw new NotImplementedException();
        }

        public int Purge()
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Purge(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.Purge();
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Purge() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Purge method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return -1;
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Restore(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.Restore(snapshot);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Restore() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return null;
                }
            }
        }

        public void Select(string parameters)
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Restore(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    this.service.Select(parameters);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Select() was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                }
            }
        }

        public void Update(string parameters)
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Restore(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    this.service.Update(parameters);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Update() was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                }
            }
        }
    }
}
