using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FileCabinetApp
{
    /// <summary>
    /// Meters time.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Parameter to initialize service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record and return id.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to insert data.</param>
        /// <returns>Record id.</returns>
        public int CreateRecord(FileCabinetRecord fileCabinetRecord)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.CreateRecord(fileCabinetRecord);
            stopWatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Deletes record.
        /// </summary>
        /// <param name="name">Parameter to find parameter to delete record.</param>
        /// <param name="value">Parameter to delete record.</param>
        public void Delete(string name, string value)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            this.service.Delete(name, value);
            stopWatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Record count.</returns>
        public (int, int) GetStat()
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.GetStat();
            stopWatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Inserts new record and return id.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to insert data.</param>
        /// <returns>Record id.</returns>
        public int Insert(FileCabinetRecord fileCabinetRecord)
        {
            return this.CreateRecord(fileCabinetRecord);
        }

        /// <summary>
        /// Purges file from deleted records.
        /// </summary>
        /// <returns>Result of action.</returns>
        public int Purge()
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.Purge();
            stopWatch.Stop();
            Console.WriteLine($"Purge method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Creates snapshot.
        /// </summary>
        /// <param name="snapshot">Parameter to restore data.</param>
        /// <returns>Snapshot.</returns>
        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            var result = this.service.Restore(snapshot);
            stopWatch.Stop();
            Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <summary>
        /// Selects record.
        /// </summary>
        /// <param name="parameters">Parameter select record.</param>
        public void Select(string parameters)
        {
            Stopwatch stopWatch = new ();
            stopWatch.Start();
            this.service.Select(parameters);
            stopWatch.Stop();
            Console.WriteLine($"Update method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Updates record.
        /// </summary>
        /// <param name="parameters">Parameter update record.</param>
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
