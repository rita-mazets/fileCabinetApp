using System;
using System.Globalization;
using FileCabinetApp;

namespace FileCabinetApp
{
    /// <summary>
    /// Starting class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Margarita Mazets";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the amount of data in the service", "The 'stat' command print the amount of data in the service. " },
            new string[] { "create", "creates new record", "The 'create' command creates new record. " },
            new string[] { "list", "prints all records", "The 'list' command prints all records. " },
            new string[] { "edit", "allows to edit record ", "The 'edit' command allows to edit record. " },
            new string[] { "find", "allows to find record ", "The 'find' command allows to find record. " },
        };

        /// <summary>
        /// The main program method.
        /// </summary>
        /// <param name="args">Sets command line parameters.</param>
        public static void Main(string[] args)
        {
            string nameValidationParam = ParseArgs(args);

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {nameValidationParam} validation rules.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static string ParseArgs(string[] argc)
        {
            bool isValidationRules = false;
            bool isV = false;
            string nameValidationParam = "default";
            if (argc is not null)
            {
                foreach (var item in argc)
                {
                    if (item.ToLower(CultureInfo.CurrentCulture).Contains("--validation-rules="))
                    {
                        if (item.ToLower(CultureInfo.CurrentCulture).Contains("default"))
                        {
                            fileCabinetService = new FileCabinetDefaultService();
                            isValidationRules = true;
                            nameValidationParam = "default";
                        }

                        if (item.ToLower(CultureInfo.CurrentCulture).Contains("custom"))
                        {
                            isValidationRules = true;
                            fileCabinetService = new FileCabinetCustomService();
                            nameValidationParam = "custom";
                        }
                    }

                    if (item.ToLower(CultureInfo.CurrentCulture).Equals("-v"))
                    {
                        isV = true;
                    }

                    if (isV)
                    {
                        if (item.ToLower(CultureInfo.CurrentCulture).Equals("default"))
                        {
                            fileCabinetService = new FileCabinetDefaultService();
                            isValidationRules = true;
                            nameValidationParam = "default";
                            isV = false;
                        }

                        if (item.ToLower(CultureInfo.CurrentCulture).Contains("custom"))
                        {
                            isValidationRules = true;
                            fileCabinetService = new FileCabinetCustomService();
                            nameValidationParam = "custom";
                            isV = false;
                        }
                    }
                }
            }

            if (!isValidationRules)
            {
                fileCabinetService = new FileCabinetDefaultService();
                nameValidationParam = "default";
            }

            return nameValidationParam;
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            try
            {
                var fileCabinetRecord = Program.Input();
                var result = Program.fileCabinetService.CreateRecord(fileCabinetRecord);
                Console.WriteLine($"Record #{result} is created.\n");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Incorrect data! Please, input data again!");
                Program.Create(parameters);
            }

            Console.WriteLine();
        }

        private static FileCabinetRecord Input()
        {
            string dateString, typeString;
            string firstName, lastName;
            DateTime date;
            short height;
            decimal salary;
            char type;

            Console.Write("First name:");
            firstName = Console.ReadLine();

            Console.Write("Last name:");
            lastName = Console.ReadLine();

            do
            {
                Console.Write("Date of birth:");
                dateString = Console.ReadLine();
            }
            while (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out date));

            do
            {
                Console.Write("Height:");
            }
            while (!short.TryParse(Console.ReadLine(), out height));

            do
            {
                Console.Write("Salary:");
            }
            while (!decimal.TryParse(Console.ReadLine(), out salary));

            Console.Write("Type:");
            typeString = Console.ReadLine();
            type = typeString[0];

            return new FileCabinetRecord { FirstName = firstName, LastName = lastName, DateOfBirth = date, Height = height, Salary = salary, Type = type };
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}");
            }
        }

        private static void Edit(string parameters)
        {
            int id;

            if (!int.TryParse(parameters, out id))
            {
                Console.WriteLine("Incorrect id parameter");
                return;
            }

            try
            {
                var fileCabinetRecord = Program.Input();
                fileCabinetRecord.Id = id;

                Program.fileCabinetService.EditRecord(fileCabinetRecord);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }

        private static void Find(string parameters)
        {
            FileCabinetRecord[] records = Array.Empty<FileCabinetRecord>();
            var parametersArray = parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parametersArray.Length < 2)
            {
                throw new ArgumentException("'find' must have more than 2 parameters", nameof(parameters));
            }

            string command = parametersArray[0].ToLower(CultureInfo.CurrentCulture);

            if (command == "firstname")
            {
                try
                {
                    string firstName = parametersArray[1];
                    firstName = firstName[1..^2];
                    records = Program.fileCabinetService.FindByFirstName(firstName);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Record with that firstname not found");
                }
            }

            if (command == "lastname")
            {
                try
                {
                    string lastname = parametersArray[1];
                    lastname = lastname[1..^2];
                    records = Program.fileCabinetService.FindByLastName(lastname);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Record with that lasrname not found");
                }
            }

            if (command == "dateofbirth")
            {
                try
                {
                    string dateofbirth = parametersArray[1];
                    dateofbirth = dateofbirth[1..^2];
                    DateTime date;
                    if (!DateTime.TryParseExact(dateofbirth, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out date))
                    {
                        Console.WriteLine("Incorrect date");
                        return;
                    }

                    records = Program.fileCabinetService.FindDateOfBirth(date);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Record with that firstname not found");
                }
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}");
            }

            Console.WriteLine();
        }
    }
}