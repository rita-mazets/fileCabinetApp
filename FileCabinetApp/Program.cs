using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using FileCabinetApp;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Starting class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Margarita Mazets";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// The main program method.
        /// </summary>
        /// <param name="args">Sets command line parameters.</param>
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("validate-rule.json", true, true).Build();
            string nameValidationParam, nameStorageParam;
            (nameValidationParam, nameStorageParam) = ParseArgs(args, config);

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
            var exitHandler = new ExitCommandHandler(Program.ChangeRunning);
            var exportHandler = new ExportCommandHandler(Program.fileCabinetService);
            var findHandler = new FindCommandHandler(Program.fileCabinetService, DefaultRecordPrint);
            var importHandler = new ImportCommandHandler(Program.fileCabinetService, recordPrinter);
            var listHandler = new ListCommandHandler(Program.fileCabinetService, recordPrinter);
            var purgeHandler = new PurgeCommandHandler(Program.fileCabinetService);
            var statHandler = new StatCommandHandler(Program.fileCabinetService);
            var insertHandler = new InsertCommandHandler(Program.fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(Program.fileCabinetService);
            var updateHandler = new UpdateCommandHandler(Program.fileCabinetService);
            var errorHandler = new ErrorCommandHandler(Program.fileCabinetService);

            helpHandler.SetNext(createHandler).SetNext(exitHandler).SetNext(exportHandler).SetNext(findHandler).SetNext(importHandler).SetNext(listHandler).SetNext(purgeHandler).SetNext(statHandler).SetNext(insertHandler).SetNext(deleteHandler).SetNext(updateHandler).SetNext(errorHandler);

            return helpHandler;
        }

        private static (string, string) ParseArgs(string[] argc, IConfiguration config)
        {
            bool isValidationRules = false;
            bool isStorageRules = false;
            bool isUseStopWatch = false;
            bool isUseLogger = false;
            bool isV = false;
            bool isS = false;
            string nameValidationParam = "default";
            string nameStorageParam = "memory";

            var validateParamDefault = ConvertToValidateParam(config, "default");
            var validateParamCustom = ConvertToValidateParam(config, "custom");

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

                    if (argc[i] == "-use-stopwatch")
                    {
                        isUseStopWatch = true;
                        Console.WriteLine("Use StopWatch");
                    }

                    if (argc[i] == "-use-logger")
                    {
                        isUseLogger = true;
                        Console.WriteLine("Use Logger");
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
                    var validator = new ValidatorBuilder().CreateDefault(validateParamDefault);
                    fileCabinetService = new FileCabinetMemoryService(validator);
                }

                if (nameValidationParam.Equals("custom"))
                {
                    var validator = new ValidatorBuilder().CreateCustom(validateParamCustom);
                    fileCabinetService = new FileCabinetMemoryService(validator);
                }
            }

            if (nameStorageParam.Equals("file"))
            {
                if (nameValidationParam.Equals("default"))
                {
                    var validator = new ValidatorBuilder().CreateDefault(validateParamDefault);
                    fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate), validator);
                }

                if (nameValidationParam.Equals("custom"))
                {
                    var validator = new ValidatorBuilder().CreateCustom(validateParamCustom);
                    fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate), validator);
                }
            }

            if (isUseStopWatch)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (isUseLogger)
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
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

        private static ValidateParam ConvertToValidateParam(IConfiguration config, string validator)
        {
            var validateParam = new ValidateParam();

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            validateParam.FirstNameMin = config[validator + ":firstname:min"] is not null ? Convert.ToInt32(config[validator + ":firstname:min"], CultureInfo.CurrentCulture) : 0;
            validateParam.FirstNameMax = config[validator + ":firstname:max"] is not null ? Convert.ToInt32(config[validator + ":firstname:max"], CultureInfo.CurrentCulture) : int.MaxValue;
            validateParam.LastNameMin = config[validator + ":lastname:min"] is not null ? Convert.ToInt32(config[validator + ":lastname:min"], CultureInfo.CurrentCulture) : 0;
            validateParam.LastNameMax = config[validator + ":lastname:max"] is not null ? Convert.ToInt32(config[validator + ":firstname:max"], CultureInfo.CurrentCulture) : int.MaxValue;
            validateParam.DateOfBirthFrom = config[validator + ":dateOfbirth:from"] is not null ? Convert.ToDateTime(config[validator + ":dateOfbirth:from"], CultureInfo.CurrentCulture) : new DateTime(1900);
            validateParam.DateOfBirthTo = config[validator + ":dateOfbirth:to"] is not null ? Convert.ToDateTime(config[validator + ":dateOfbirth:to"], CultureInfo.CurrentCulture) : DateTime.Now;
            validateParam.HeightMin = config[validator + ":height:min"] is not null ? Convert.ToInt32(config[validator + ":height:min"], CultureInfo.CurrentCulture) : 0;
            validateParam.HeightMax = config[validator + ":height:max"] is not null ? Convert.ToInt32(config[validator + ":height:max"], CultureInfo.CurrentCulture) : short.MaxValue;
            validateParam.SalaryMin = config[validator + ":salary:min"] is not null ? Convert.ToInt32(config[validator + ":salary:min"], CultureInfo.CurrentCulture) : 0;

            return validateParam;
        }
    }
}