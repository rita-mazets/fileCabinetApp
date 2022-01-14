using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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
