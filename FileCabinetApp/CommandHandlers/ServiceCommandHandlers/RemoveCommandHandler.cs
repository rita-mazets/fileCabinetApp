using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {

        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("remove"))
            {
                int id;

                if (!int.TryParse(appComandRequest.Peremeters, out id))
                {
                    return "Incorrect id parameter";
                }

                try
                {
                    this.fileCabinetService.Remove(id);
                    return $"Record {id} was deleted";
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
