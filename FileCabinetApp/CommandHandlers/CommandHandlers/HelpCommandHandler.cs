using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Help method.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the amount of data in the service", "The 'stat' command print the amount of data in the service. " },
            new string[] { "create", "creates new record", "The 'create' command creates new record. " },
            new string[] { "update", "allows to edit record ", "The 'update' command allows to edit record. " },
            new string[] { "select", "allows to find record ", "The 'select' command allows to find record. " },
            new string[] { "export", "exports to csv or xml ", "The 'export' command exports to csv or xml " },
            new string[] { "import", "import from csv or xml ", "The 'import' command import from csv or xml " },
            new string[] { "delete", "delete from file ", "The 'delete' command delete record " },
            new string[] { "purge", "purge date ", "The 'purge' command purge data " },
        };

#pragma warning disable SA1203 // Constants should appear before fields
        private const int CommandHelpIndex = 0;
#pragma warning restore SA1203 // Constants should appear before fields
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

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

            if (appComandRequest.Command.Equals("help"))
            {
                if (!string.IsNullOrEmpty(appComandRequest.Peremeters))
                {
                    var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], appComandRequest.Peremeters, StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                       return helpMessages[index][ExplanationHelpIndex];
                    }
                    else
                    {
                        return $"There is no explanation for '{appComandRequest.Peremeters}' command.";
                    }
                }
                else
                {
                    var result = "Available commands:\n";

                    foreach (var helpMessage in helpMessages)
                    {
                        result += $"\t{helpMessage[CommandHelpIndex]}\t- {helpMessage[DescriptionHelpIndex]}\n";
                    }

                    return result;
                }
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }
    }
}
