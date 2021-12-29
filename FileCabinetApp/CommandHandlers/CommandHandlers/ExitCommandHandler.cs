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

        public ExitCommandHandler(bool isRunning)
        {
            this.isRunning = isRunning;
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("exit"))
            {
                this.running = ChangeRunning;
                this.running(this.isRunning);
                return "Exiting an application...";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }

        private static void ChangeRunning(bool isRunning)
        {
            isRunning = false;
        }
    }
}
