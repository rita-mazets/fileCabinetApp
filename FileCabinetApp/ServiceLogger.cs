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

        public void EditRecord(FileCabinetRecord fileCabinetRecord)
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
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Edit() with FirsrName = \"{fileCabinetRecord.FirstName}\", LastName = \"{fileCabinetRecord.LastName}\", DateOfBirth = \"{fileCabinetRecord.DateOfBirth}\", Height = \"{fileCabinetRecord.Height}\", Salary = \"{fileCabinetRecord.Salary}\", Type = \"{fileCabinetRecord.Type}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    this.service.EditRecord(fileCabinetRecord);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Edit() was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"EditRecord method execution duration is {stopWatch.ElapsedTicks} ticks.");
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling FindByFirstName() with FirsrName = \"{firstName}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.FindByFirstName(firstName);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} FindByFirstName() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"FindByFirstName method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return null;
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling FindByFirstName() with LastName = \"{lastName}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.FindByLastName(lastName);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} FindByLastName() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"FindByLastName method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return null;
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling FindDateOfBirth() with DateOfBirth = \"{dateOfBirth:dd:MM:yyyy}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.FindDateOfBirth(dateOfBirth);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} FindDateOfBirth() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"FindByLastName method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return null;
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling GetRecords(). ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    var result = this.service.GetRecords();
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} GetRecords() returned \"{result}\" and was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"GetRecords method execution duration is {stopWatch.ElapsedTicks} ticks.");
                    return result;
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
                    return null;
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

        public void Remove(int id)
        {
            using (var stream = new StreamWriter(this.fileName, true))
            {
                try
                {
                    Stopwatch stopWatch = new ();
                    var text = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Calling Remove() with id = \"{id}\". ";
                    stream.WriteLine(text);

                    stopWatch.Start();
                    this.service.Remove(id);
                    stopWatch.Stop();
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Purge() was completed in time {stopWatch.ElapsedTicks} ticks");

                    Console.WriteLine($"Remove method execution duration is {stopWatch.ElapsedTicks} ticks.");
                }
                catch (Exception e)
                {
                    stream.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Exeption: {e.Message}");
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
    }
}
