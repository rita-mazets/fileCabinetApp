using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandlerBase:ICommandHandler
    {
        private ICommandHandler nextHandler;

        public virtual object Handle(AppComandRequest appComandRequest) 
        {
            if (this.nextHandler is null)
            {
                return null;
            }
            else
            {
                return this.nextHandler.Handle(appComandRequest);
            }

        }

        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
            return commandHandler;
        }
    }
}
