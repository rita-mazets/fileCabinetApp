using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {

        public StatCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("stat"))
            {
                var (recordsCount, deletedRec) = this.fileCabinetService.GetStat();
                return $"{recordsCount} record(s).\n{deletedRec} deleted.";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
