using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class InputDate
    {
        public static FileCabinetRecord Input()
        {
            string firstName, lastName;
            DateTime date;
            short height;
            decimal salary;
            char type;

            Console.Write("First name:");
            firstName = ReadInput<string>(DataConverter.StringConverter, DataValidator.NameValidator);

            Console.Write("Last name:");
            lastName = ReadInput<string>(DataConverter.StringConverter, DataValidator.NameValidator);

            Console.Write("Date of birth:");
            date = ReadInput<DateTime>(DataConverter.DateConverter, DataValidator.DateValidator);

            Console.Write("Height:");
            height = ReadInput<short>(DataConverter.ShortConverter, DataValidator.HeightValidator);

            Console.Write("Salary:");
            salary = ReadInput<decimal>(DataConverter.DecimalConverter, DataValidator.SalaryValidator);

            Console.Write("Type:");
            type = ReadInput<char>(DataConverter.CharConverter, DataValidator.TypeValidator);

            return new FileCabinetRecord { FirstName = firstName, LastName = lastName, DateOfBirth = date, Height = height, Salary = salary, Type = type };
        }

        private static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
