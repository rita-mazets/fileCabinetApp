using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Import method.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        /// <param name="printer">Parameter to initialize printer.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Perform action.
        /// </summary>
        /// <param name="appComandRequest">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("import"))
            {
                var parametersArray = appComandRequest.Peremeters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parametersArray.Length < 2)
                {
                    throw new ArgumentException("'export' must have more than 2 parameters", nameof(appComandRequest));
                }

                string command = parametersArray[0].ToLower(CultureInfo.CurrentCulture);
                string filePath = parametersArray[1];

                if (command.ToLower(CultureInfo.CurrentCulture) == "csv")
                {
                    return this.ImportCsv(filePath);
                }

                if (command.ToLower(CultureInfo.CurrentCulture) == "xml")
                {
                    return this.ImportXml(filePath);
                }

                return "Records was not importing";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }

        private string ImportCsv(string filePath)
        {
            ReadOnlyCollection<FileCabinetRecord> records;
            var snapshot = new FileCabinetServiceSnapshot();
            using (var stream = new StreamReader(filePath))
            {
                snapshot.LoadFromCsvFile(stream);
                records = this.fileCabinetService.Restore(snapshot);
            }

            string result = "This records was writing in file:\n";
            foreach (var record in records)
            {
                result += $"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}\n";
            }

            return result;
        }

        private string ImportXml(string filePath)
        {
            ReadOnlyCollection<FileCabinetRecord> records;
            var snapshot = new FileCabinetServiceSnapshot();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                snapshot.LoadFromXmlFile(stream);
                records = this.fileCabinetService.Restore(snapshot);
            }

            string result = "This records was writing in file:\n" + this.printer.Print(records);
            return result;
        }
    }
}
