using System;
using System.Globalization;
using FileCabinetApp;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Margarita Mazets";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService = new FileCabinetService();

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

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            string firstName, lastName;
            DateTime date;
            short height;
            decimal salary;
            char type;

            Program.Input(out firstName, out lastName, out date, out height, out salary, out type);

            var result = Program.fileCabinetService.CreateRecord(firstName, lastName, date, height, salary, type);
            Console.WriteLine($"Record #{result} is created.\n");
        }

        private static void Input(out string firstName, out string lastName, out DateTime date, out short height, out decimal salary, out char type)
        {
            string dateString, typeString;
            do
            {
                Console.Write("First name:");
                firstName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60);

            do
            {
                Console.Write("Last name:");
                lastName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60);

            do
            {
                Console.Write("Date of birth:");
                dateString = Console.ReadLine();
            }
            while (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out date) || date < new DateTime(1950, 1, 1) || date > DateTime.Now);

            do
            {
                Console.Write("Height:");
            }
            while (!short.TryParse(Console.ReadLine(), out height) || height < 0 || height > 250);

            do
            {
                Console.Write("Salary:");
            }
            while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0);

            do
            {
                Console.Write("Type:");
                typeString = Console.ReadLine();
                type = typeString[0];
            }
            while (!char.IsLetter(type));
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
            string firstName, lastName;
            DateTime date;
            short height;
            decimal salary;
            char type;
            int id;

            while (!int.TryParse(parameters, out id))
            {
            }

            Program.Input(out firstName, out lastName, out date, out height, out salary, out type);
            try
            {
                Program.fileCabinetService.EditRecord(id, firstName, lastName, date, height, salary, type);
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
                string firstName = parametersArray[1];
                firstName = firstName.Substring(1, firstName.Length - 2);
                records = Program.fileCabinetService.FindByFirstName(firstName);
            }

            if (command == "lastname")
            {
                string lastname = parametersArray[1];
                lastname = lastname.Substring(1, lastname.Length - 2);
                records = Program.fileCabinetService.FindByLastName(lastname);
            }

            if (command == "dateofbirth")
            {
                string dateofbirth = parametersArray[1];
                dateofbirth = dateofbirth.Substring(1, dateofbirth.Length - 1);
                records = Program.fileCabinetService.FindByFirstName(dateofbirth);
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.Height}, {record.Salary}, {record.Type}");
            }
        }
    }
}