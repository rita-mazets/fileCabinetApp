using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("purge"))
            {
                var count = Program.fileCabinetService.Purge();
                return $"Data file processing is completed: {count} were purged.";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
