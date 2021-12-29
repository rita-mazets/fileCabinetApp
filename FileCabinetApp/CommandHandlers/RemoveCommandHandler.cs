using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : CommandHandlerBase
    {
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
                    Program.fileCabinetService.Remove(id);
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
