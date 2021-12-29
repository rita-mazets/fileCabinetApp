using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler: CommandHandlerBase
    {
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

                    Program.fileCabinetService.EditRecord(fileCabinetRecord);
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
