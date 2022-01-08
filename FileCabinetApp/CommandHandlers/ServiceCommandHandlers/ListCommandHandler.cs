using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;
using FileCabinetApp.interfaces;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public ListCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
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

            if (appComandRequest.Command.Equals("list"))
            {
                var records = this.fileCabinetService.GetRecords();
                return this.printer.Print(records);
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
