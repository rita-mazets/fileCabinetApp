using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler: ServiceCommandHandlerBase
    {
        public EditCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("edit"))
            {
                int id;

                if (!int.TryParse(appComandRequest.Peremeters, out id))
                {
                    return "Incorrect id parameter";
                }

                try
                {
                    var fileCabinetRecord = InputDate.Input();
                    fileCabinetRecord.Id = id;

                    this.fileCabinetService.EditRecord(fileCabinetRecord);
                    return $"Record #{id} is updated.";
                }
                catch (ArgumentException)
                {
                    return $"#{id} record is not found.";
                }
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
