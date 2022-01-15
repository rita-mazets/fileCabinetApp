using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Select method.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appComandRequest.Command.Equals("select"))
            {
                try
                {
                    var param = appComandRequest.Peremeters.ToLower(CultureInfo.CurrentCulture);
                    this.fileCabinetService.Select(param);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return string.Empty;
            }

            return base.Handle(appComandRequest);
        }
    }
}
