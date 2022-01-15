using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create base commandHandler.
    /// </summary>
    public class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>
        /// Perform action.
        /// </summary>
        /// <param name="appComandRequest">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
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

        /// <summary>
        /// Set value to nextHandler.
        /// </summary>
        /// <param name="commandHandler">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
            return commandHandler;
        }
    }
}
