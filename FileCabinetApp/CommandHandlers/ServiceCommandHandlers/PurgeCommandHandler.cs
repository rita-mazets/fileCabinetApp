using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("purge"))
            {
                var count = this.fileCabinetService.Purge();
                return $"Data file processing is completed: {count} were purged.";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
