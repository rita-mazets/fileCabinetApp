using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("stat"))
            {
                var (recordsCount, deletedRec) = Program.fileCabinetService.GetStat();
                return $"{recordsCount} record(s).\n{deletedRec} deleted.";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
