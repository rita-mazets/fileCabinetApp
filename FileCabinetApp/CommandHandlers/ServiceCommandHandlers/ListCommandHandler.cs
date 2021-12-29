using System;
using System.Collections.Generic;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("list"))
            {
                var records = this.fileCabinetService.GetRecords();
                var result = string.Empty;

                foreach (var record in records)
                {
                    result += $"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}\n";
                }

                return result;
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
