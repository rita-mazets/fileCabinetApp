using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;
using FileCabinetApp.interfaces;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public FindCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("find"))
            {
                IEnumerable<FileCabinetRecord> records = Array.Empty<FileCabinetRecord>();
                var parametersArray = appComandRequest.Peremeters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parametersArray.Length < 2)
                {
                    throw new ArgumentException("'find' must have more than 2 parameters", nameof(appComandRequest));
                }

                string command = parametersArray[0].ToLower(CultureInfo.CurrentCulture);

                if (command == "firstname")
                {
                    try
                    {
                        string firstName = parametersArray[1];
                        firstName = firstName[1..^1];
                        records = this.fileCabinetService.FindByFirstName(firstName);
                    }
                    catch (ArgumentException)
                    {
                        return "Record with that firstname not found";
                    }
                }

                if (command == "lastname")
                {
                    try
                    {
                        string lastname = parametersArray[1];
                        lastname = lastname[1..^1];
                        records = this.fileCabinetService.FindByLastName(lastname);
                    }
                    catch (ArgumentException)
                    {
                        return "Record with that lastname not found";
                    }
                }

                if (command == "dateofbirth")
                {
                    try
                    {
                        string dateofbirth = parametersArray[1];
                        dateofbirth = dateofbirth[1..^1];
                        DateTime date;
                        if (!DateTime.TryParseExact(dateofbirth, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out date))
                        {
                            return "Incorrect date";
                        }

                        records = this.fileCabinetService.FindDateOfBirth(date);
                    }
                    catch (ArgumentException)
                    {
                        return "Record with that firstname not found";
                    }
                }

                return this.printer.Print(records);
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
