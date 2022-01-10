using System;
using System.Globalization;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {

        public DeleteCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("delete"))
            {
                try
                {
                    var parameters = appComandRequest.Peremeters.Split(" ");

                    if (!parameters[0].ToLower(CultureInfo.CurrentCulture).Equals("where"))
                    {
                        throw new ArgumentException("Incorrect syntax: not found word \'where\'");
                    }

                    this.fileCabinetService.Delete(parameters[1].ToLower(CultureInfo.CurrentCulture), parameters[3].ToLower(CultureInfo.CurrentCulture).Trim('\''));

                    return "dd";
                }
                catch (Exception e)
                {
                    Console.WriteLine("Data is not correct");
                    return string.Empty;
                }
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
