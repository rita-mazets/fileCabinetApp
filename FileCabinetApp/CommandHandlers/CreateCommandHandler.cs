using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler:CommandHandlerBase
    {
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
                    var result = Program.fileCabinetService.CreateRecord(fileCabinetRecord);
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
