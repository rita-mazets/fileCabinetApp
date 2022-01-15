using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Exit method.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> running;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="running">Parameter to set running.</param>
        public ExitCommandHandler(Action<bool> running)
        {
            this.running = running;
        }

        /// <summary>
        /// Perform action.
        /// </summary>
        /// <param name="appComandRequest">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
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
