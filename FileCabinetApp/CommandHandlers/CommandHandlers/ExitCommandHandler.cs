using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> running;
        private bool isRunning;

        public ExitCommandHandler(Action<bool> running)
        {
            this.running = running;
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("exit"))
            {
                this.running(false);
                return "Exiting an application...";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
