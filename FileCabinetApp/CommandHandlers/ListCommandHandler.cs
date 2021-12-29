using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : CommandHandlerBase
    {
        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("list"))
            {
                var records = Program.fileCabinetService.GetRecords();
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
