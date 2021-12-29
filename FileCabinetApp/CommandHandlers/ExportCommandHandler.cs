using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
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
                    return ExportCsv(filePath);
                }

                if (command.ToLower(CultureInfo.CurrentCulture) == "xml")
                {
                    return ExportXml(filePath);
                }

                return "File wasn't writting";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }

        private static string ExportCsv(string filePath)
        {
            try
            {
                using (StreamWriter sw = new (filePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Id,FirstName,LastName,DateOfBirth,Height,Salary,Type");

                    var fileCabinetService1 = (FileCabinetMemoryService)Program.fileCabinetService;
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

        private static string ExportXml(string filePath)
        {
            try
            {
                XmlWriterSettings settings = new ();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (XmlWriter xw = XmlWriter.Create(filePath, settings))
                {
                    var fileCabinetService1 = (FileCabinetMemoryService)Program.fileCabinetService;
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