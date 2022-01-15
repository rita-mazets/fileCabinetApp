using System;
using System.Globalization;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Delete method.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public DeleteCommandHandler(IFileCabinetService fileCabinetService)
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

                    return string.Empty;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Data is not correct");
                    Console.WriteLine(e.Message);
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
