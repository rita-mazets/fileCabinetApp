using System;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Create method.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appComandRequest.Command.Equals("create"))
            {
                try
                {
                    var fileCabinetRecord = InputDate.Input();
                    var result = this.fileCabinetService.CreateRecord(fileCabinetRecord);
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
