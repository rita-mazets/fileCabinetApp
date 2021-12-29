using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using FileCabinetApp;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Starting class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Margarita Mazets";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        //private const int CommandHelpIndex = 0;
        //private const int DescriptionHelpIndex = 1;
        //private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// The main program method.
        /// </summary>
        /// <param name="args">Sets command line parameters.</param>
        public static void Main(string[] args)
        {
            string nameValidationParam, nameStorageParam;
            (nameValidationParam, nameStorageParam) = ParseArgs(args);

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {nameValidationParam} validation rules.");
            Console.WriteLine($"Using {nameStorageParam} cabinet service.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                const int parametersIndex = 1;
                var command = inputs[commandIndex];
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var commandHandler = CreateCommandHandlers();
                var result = commandHandler.Handle(new AppComandRequest(command, parameters));
                Console.WriteLine(result);
            }
            while (isRunning);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordPrinter = new DefaultRecordPrinter();
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(Program.fileCabinetService);
            var editHandler = new EditCommandHandler(Program.fileCabinetService);
            var exitHandler = new ExitCommandHandler(Program.ChangeRunning);
            var exportHandler = new ExportCommandHandler(Program.fileCabinetService);
            var findHandler = new FindCommandHandler(Program.fileCabinetService, DefaultRecordPrint);
            var importHandler = new ImportCommandHandler(Program.fileCabinetService, recordPrinter);
            var listHandler = new ListCommandHandler(Program.fileCabinetService, recordPrinter);
            var purgeHandler = new PurgeCommandHandler(Program.fileCabinetService);
            var removeHandler = new RemoveCommandHandler(Program.fileCabinetService);
            var statHandler = new StatCommandHandler(Program.fileCabinetService);

            helpHandler.SetNext(createHandler).SetNext(editHandler).SetNext(exitHandler).SetNext(exportHandler).SetNext(findHandler).SetNext(importHandler).SetNext(listHandler).SetNext(purgeHandler).SetNext(removeHandler).SetNext(statHandler);

            return helpHandler;
        }

        private static (string, string) ParseArgs(string[] argc)
        {
            bool isValidationRules = false;
            bool isStorageRules = false;
            bool isV = false;
            bool isS = false;
            string nameValidationParam = "default";
            string nameStorageParam = "memory";

            if (argc is not null)
            {
                for (int i = 0; i < argc.Length; i++)
                {
                    (nameValidationParam, isValidationRules, isV) = CheckParam(argc[i], isValidationRules, isV, "--validation-rules=", "-v", "default", "custom", nameValidationParam);
                    if (i + 1 < argc.Length && isV && !isValidationRules)
                    {
                        (nameValidationParam, isValidationRules, isV) = CheckParam(argc[i + 1], isValidationRules, isV, "--validation-rules=", "-v", "default", "custom", nameValidationParam);
                        if (!string.IsNullOrEmpty(nameValidationParam))
                        {
                            i++;
                        }
                    }

                    (nameStorageParam, isStorageRules, isS) = CheckParam(argc[i], isStorageRules, isS, "--storage=", "-s", "memory", "file", nameStorageParam);
                }
            }

            if (!isValidationRules)
            {
                nameValidationParam = "default";
            }

            if (string.IsNullOrEmpty(nameStorageParam) || nameStorageParam.Equals("memory"))
            {
                if (nameValidationParam.Equals("default"))
                {
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                }

                if (nameValidationParam.Equals("custom"))
                {
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                }
            }

            if (nameStorageParam.Equals("file"))
            {
                if (nameValidationParam.Equals("default"))
                {
                    fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate), new DefaultValidator());
                }

                if (nameValidationParam.Equals("custom"))
                {
                    fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate), new CustomValidator());
                }
            }

            return (nameValidationParam, nameStorageParam);
        }

        private static (string, bool, bool) CheckParam(string item, bool isRule, bool isShort, string fullNameParam, string shortNameParam, string regime1, string regime2, string nameParam)
        {
            if (item.ToLower(CultureInfo.CurrentCulture).Contains(fullNameParam))
            {
                if (item.ToLower(CultureInfo.CurrentCulture).Contains(regime1))
                {
                    isRule = true;
                    nameParam = regime1;
                }

                if (item.ToLower(CultureInfo.CurrentCulture).Contains(regime2))
                {
                    isRule = true;
                    nameParam = regime2;
                }
            }

            if (item.ToLower(CultureInfo.CurrentCulture).Equals(shortNameParam))
            {
                isShort = true;
            }

            if (isShort)
            {
                if (item.ToLower(CultureInfo.CurrentCulture).Equals(regime1))
                {
                    isRule = true;
                    nameParam = regime1;
                    isShort = false;
                }

                if (item.ToLower(CultureInfo.CurrentCulture).Contains(regime2))
                {
                    isRule = true;
                    nameParam = regime2;
                    isShort = false;
                }
            }

            return (nameParam, isRule, isShort);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}\n");
            }
        }

        private static void ChangeRunning(bool isRunning)
        {
            Program.isRunning = isRunning;
        }
    }
}