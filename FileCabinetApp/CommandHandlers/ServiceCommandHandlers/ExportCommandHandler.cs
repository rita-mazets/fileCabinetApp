using System;
using System.Globalization;
using System.IO;
using System.Xml;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Export method.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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

            if (appComandRequest.Command.Equals("export"))
            {
                var parametersArray = appComandRequest.Peremeters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parametersArray.Length < 2)
                {
                    throw new ArgumentException("'export' must have more than 2 parameters", nameof(appComandRequest));
                }

                string command = parametersArray[0].ToLower(CultureInfo.CurrentCulture);
                string filePath = parametersArray[1];

                if (!IsRewrite(filePath))
                {
                    return "Writing from file was stopped";
                }

                if (command.ToLower(CultureInfo.CurrentCulture) == "csv")
                {
                    return this.ExportCsv(filePath);
                }

                if (command.ToLower(CultureInfo.CurrentCulture) == "xml")
                {
                    return this.ExportXml(filePath);
                }

                return "File wasn't writting";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }

        private string ExportCsv(string filePath)
        {
            try
            {
                using (StreamWriter sw = new (filePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Id,FirstName,LastName,DateOfBirth,Height,Salary,Type");

                    var fileCabinetService1 = (FileCabinetMemoryService)this.fileCabinetService;
                    var snapshot = fileCabinetService1.MakeSnapshot();
                    snapshot.SaveToCsv(sw);
                    return $"All records are exported to file {filePath}.";
                }
            }
            catch (DirectoryNotFoundException)
            {
                return $"Export failed: can't open file {filePath}.";
            }
        }

        private string ExportXml(string filePath)
        {
            try
            {
                XmlWriterSettings settings = new ();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (XmlWriter xw = XmlWriter.Create(filePath, settings))
                {
                    var fileCabinetService1 = (FileCabinetMemoryService)this.fileCabinetService;
                    var snapshot = fileCabinetService1.MakeSnapshot();
                    snapshot.SaveToXml(xw);
                    return $"All records are exported to file {filePath}.";
                }
            }
            catch (DirectoryNotFoundException)
            {
                return $"Export failed: can't open file {filePath}.";
            }
        }

        private static bool IsRewrite(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string result = string.Empty;

            if (File.Exists(path))
            {
                Console.WriteLine("File is exist - rewrite e:\filename.csv?[Y / n]");
                result = Console.ReadLine();
            }

            if (result.ToLower(CultureInfo.CurrentCulture) == "y" || result.Length == 0)
            {
                return true;
            }

            return false;
        }
    }
}