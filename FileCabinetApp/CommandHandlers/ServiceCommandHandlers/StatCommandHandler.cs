using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Stat method.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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

            if (appComandRequest.Command.Equals("stat"))
            {
                var (recordsCount, deletedRec) = this.fileCabinetService.GetStat();
                return $"{recordsCount} record(s).\n{deletedRec} deleted.";
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
