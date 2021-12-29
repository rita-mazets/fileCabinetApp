using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler: ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("create"))
            {
                try
                {
                    var fileCabinetRecord = InputDate.Input();
                    var result = this.fileCabinetService.CreateRecord(fileCabinetRecord);
                    return $"Record #{result} is created.\n";
                }
                catch (ArgumentException e)
                {
                    return e.Message;
                }
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
